using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GreenGoblin : MonoBehaviour
{
    // Component references
    [SerializeField]
    private Rigidbody2D greenGoblin;
    [SerializeField]
    private GreenGoblinCombat greenGoblinCombat;
    [SerializeField]
    private PlayerMovement player;

    [SerializeField]
    private Animator animator;

    // Raycast values
    [SerializeField]
    private float rayDistance;
    [SerializeField]
    private Vector3 offset;
    private float distanceFromPlayer; // Value between goblin and player 
    private float rayOffset;

    
    private Vector2 movement;
    private Vector2 randomDirection;
    private bool canSeePlayer;

    // Ensure raycast is only returning either wall or player
    [SerializeField]
    private LayerMask layerMask;

    // Movement values
    [SerializeField]
    private float goblinSpeed = 1f;
    [SerializeField]
    private float goblinKnockback;
    [SerializeField]
    private float knockbackTime;

    
    // Values dictating how long goblins will be in idle states before transitioning
    [SerializeField]
    private int minStandTime;
    [SerializeField]
    private int maxStandTime;
    [SerializeField]
    private int minPatrolTime;
    [SerializeField]
    private int maxPatrolTime;

    // Timer related values
    private float knockbackTimer;
    private float standTimer;
    private float standTime;
    private float patrolTimer;
    private float patrolTime;

    private Pathfinding pathfinding;
    private List<Node> pathToPlayer;
    private int currentWaypoint = 0;
    public List<Vector2> waypoints;

    // Goblin States
    // Hierarchical state machine - 2 base/meta states where each base state has 2 other states to dictate the behaviour of the goblin
    // Idle state is when goblin cannot see the player                      | Aggressive state is when the goblin can see the player 

    // Base state Idle has child states of Patrol and Stand
    // Patrol makes the goblin move in a random direction for a random time | Stand makes the goblin stand still for a random time

    // Base state Aggressive has child states of Chase and Flee
    // Chase makes the goblin chase the player                              | Flee makes the goblin run away from the player if on low health 

    private enum BaseState {IDLE, AGGRESSIVE}; 

    BaseState baseState = BaseState.IDLE; // Default base state Idle
    private enum IdleState {PATROL, STAND};

    IdleState idleState = IdleState.STAND; // Default idle state stand
    private enum AggressiveState {CHASE, FLEE};

    AggressiveState aggressiveState = AggressiveState.CHASE; // Default aggressive state chase

    // Start is called before the first frame update
    void Start()
    { 
        canSeePlayer = false;
        rayOffset = 1.5f;

        // Set timers to 0 by default
        knockbackTimer = 0f;
        standTimer = 0f;
        patrolTimer = 0f;

        pathfinding = GetComponent<Pathfinding>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Continuously running timers
        standTimer -= Time.fixedDeltaTime;
        patrolTimer -= Time.fixedDeltaTime;
        knockbackTimer -= Time.fixedDeltaTime;

        // Switch statement controlling the hierarchical state machine of the goblin
        switch (baseState) {
            case BaseState.IDLE:
                Search();
                switch (idleState)
                {
                    case IdleState.PATROL:
                        Patrol();
                        break;

                    case IdleState.STAND:
                        Stand();
                        break;
                }
                break;
            case BaseState.AGGRESSIVE:
                Search();
                switch (aggressiveState)
                {
                    case AggressiveState.CHASE:

                        ChasePlayer();
                        break;

                    case AggressiveState.FLEE:
                        FleeFromPlayer();
                        break;
                }
                break;
        }
    }

    // Goblin will continuously search for the player as long as they're in a given range
    // If goblin can see player, transition to aggressive state else transition to idle state
    private void Search()
    {
        if (CanSeePlayer())
        {
            baseState = BaseState.AGGRESSIVE;
        }
        else
        {
            baseState = BaseState.IDLE;
        }
    }

    // Goblin will patrol in a random direction for a random time before transitioning to stand
    private void Patrol()
    {
        if (patrolTimer >= 0)
        {
            greenGoblin.MovePosition(greenGoblin.position + randomDirection.normalized * goblinSpeed * Time.fixedDeltaTime);
            AnimateGoblin(randomDirection.x, randomDirection.y);
        }
        else
        {
            // Goblin no longer patrolling, get random stand time and transition to stand
            standTime = Random.Range(minStandTime, maxStandTime);
            standTimer = standTime; // Set timer to random value generated between range
            idleState = IdleState.STAND;
        }
    }

    // Goblin will stand still for a random time before transitioning to patrol
    private void Stand()
    {
        if (standTimer >= 0)
        {
            greenGoblin.MovePosition(greenGoblin.position);
        }
        else
        {
            // Goblin no longer standing, get random direction, patrol time and transition to patrol
            randomDirection = new Vector2(Random.Range(-10, 10), (Random.Range(-10, 10))).normalized;
            patrolTime = Random.Range(minPatrolTime, maxPatrolTime);
            patrolTimer = patrolTime; // Set timer to random value generated between range
            idleState = IdleState.PATROL;

        }
        
    }

    // Goblin will chase player unless at 1 health (and base health is not 1) and will instead flee from player
    private void ChasePlayer()
    {
        if (knockbackTimer <= 0)
        {
            // If goblin is on 1 health, set state to flee
            if (greenGoblinCombat.GetGoblinHealth() <= 1 && greenGoblinCombat.GetGoblinBaseHealth() != 1)
            {
                aggressiveState = AggressiveState.FLEE;
            }

            pathfinding.FindPath(gameObject.transform.position, player.transform.position);
            FollowPathToPlayer();
            
        }
    }

    // Goblin will flee from player
    private void FleeFromPlayer()
    {
        if (knockbackTimer <= 0)
        {
            movement = -(player.transform.position - transform.position).normalized; // Calculate vector2 of players position from goblin and inverse to get exact opposite direction from player
            greenGoblin.MovePosition(greenGoblin.position + movement.normalized * goblinSpeed * Time.fixedDeltaTime); // Move in the opposite direction from player
            AnimateGoblin(movement.x, movement.y);
        }
    }

    // Animate goblin by using x and y values of the movement vectors 
    private void AnimateGoblin(float x, float y)
    {
        // Insert x and y values to be used by blend tree to dictate which animation to use
        animator.SetFloat("Horizontal", x);
        animator.SetFloat("Vertical", y);

        // If enemy is moving, set moving to true to run blend tree
        if (x != 0 || y != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else // Else enemy is not moving, do not run blend tree animations
        {
            animator.SetBool("isMoving", false);
        }
    }

    // Add small force to goblin facing directly away from he player
    public void Knockback()
    {
        knockbackTimer = knockbackTime; // Set knockback timer to knockbackTime
        movement = -(player.transform.position - transform.position).normalized; // Set movement to be directly away from the player
        greenGoblin.AddForce(movement * goblinKnockback, ForceMode2D.Impulse); // Add impulse force to enemy  to act as knockback
    }

    // Raycast to be used by goblin to dictate whether they can see the player or not
    private bool CanSeePlayer()
    {
        // Calculate distance between the goblin and the player
        distanceFromPlayer = Vector3.Distance(player.transform.position, greenGoblin.transform.position);
        
        // If distance between goblin and player is not roughly around or less than ray distance value, do not raycast
        // Raycast is expensive on hardware and should not be ran if unneeded
        if(distanceFromPlayer <= rayDistance + rayOffset)
        {
            Vector3 rayPosition = transform.position + offset; // Origin of ray plus offset to place near head
            Vector3 rayDirection = (player.transform.position - rayPosition).normalized; // Direction of ray always facing player

            RaycastHit2D hit;
            hit = Physics2D.Raycast(rayPosition, rayDirection, rayDistance, layerMask); // Raycast towards player

            // If player in deathzone, cannot see player
            if (hit.collider != null && hit.transform.CompareTag("DeathZone"))
            {
                Debug.Log("Hit obstacles");
                canSeePlayer = false;
                return canSeePlayer;
            }

            if (hit.collider != null) 
            {
                if (hit.transform.CompareTag("Player")){ // Raycast has hit player
                    canSeePlayer = true; // Enemy can see player
                    return canSeePlayer;
                }
                canSeePlayer = false; // Raycast has hit a wall or object in Layermask, enemy cannot see player
                return canSeePlayer;
            }
            canSeePlayer = false; // Raycast has hit nothing on Layermask, enemy cannot see player
            return canSeePlayer;
        }
        canSeePlayer = false; // Player is out of raycast range, enemy cannot see player
        return canSeePlayer;
    }

    // FollowPathToPlayer moves goblin to the nearest pathfinding node or directly towards the player if there is no node to follow
    private void FollowPathToPlayer()
    {
        // Calculate pathh to player
        pathToPlayer = pathfinding.GetPath();
        if (pathToPlayer != null && pathToPlayer.Count > 1) // If there is a path to the player, move to the next node on path
        {
            movement = new Vector2(pathToPlayer[1].worldPos.x, pathToPlayer[1].worldPos.y);
            AnimateGoblin(-(greenGoblin.position.x - movement.x), -(greenGoblin.position.y - movement.y));
        } else // Else there is no path, move directly towards the player
        {
            movement = player.transform.position;
            AnimateGoblin(-(greenGoblin.position.x - movement.x), -(greenGoblin.position.y - movement.y));
        }
        
        greenGoblin.MovePosition(greenGoblin.position - (greenGoblin.position - movement).normalized * goblinSpeed * Time.fixedDeltaTime); // Move towards set movement vector
    }
}
