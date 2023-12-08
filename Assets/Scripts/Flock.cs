using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    [SerializeField]
    private FlockAgent bee;
    [SerializeField]
    private FlockBehaviour beeBehaviour;
    [SerializeField]
    private LayerMask layerMask;
    List<FlockAgent> bees = new List<FlockAgent>();
    
    [SerializeField, Range(1,500)]
    private int startingCount = 250;
    private float agentDensity = 0.05f;
    [SerializeField, Range(1f, 10f)]
    private float neighbourRadius = 1.5f;

    [SerializeField, Range(0f, 5f)]
    private float alignmentWeight;
    [SerializeField, Range(0f, 5f)]
    private float separationWeight;
    [SerializeField, Range(0f, 5f)]
    private float cohesionWeight;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate randomly directed bees in circle
        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(bee, Random.insideUnitCircle * 200 * agentDensity + (Vector2)transform.position, Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), transform);
            newAgent.name = "Bee " + i;
            bees.Add(newAgent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // For all instantiated bees
        foreach(FlockAgent bee in bees)
        {
            // If bee out of bounds, reset position and rotation
            if(bee.transform.position.x < -10 || bee.transform.position.x > 10 || bee.transform.position.y > 10 || bee.transform.position.y < -10){
                bee.transform.position = new Vector3(transform.position.x + Random.Range(-5,5), transform.position.y + Random.Range(-5,5), 0);;
                bee.transform.rotation = Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));
            }

            // Calculate nearby bees
            List<Transform> nearbyBees = GetNearbyBees(bee);

            // Run behaviours and calculate direction based on values returned 
            Vector2 direction = new Vector2();
            direction += beeBehaviour.Alignment(bee, nearbyBees) * alignmentWeight;
            direction += beeBehaviour.Separation(bee, nearbyBees) * separationWeight;
            direction += beeBehaviour.Cohesion(bee, nearbyBees) * cohesionWeight;
            bee.MoveBee(direction); // Move bee towards direction
        }

    }

    // Get nearby bees returns a list of all bees within a specific radius of a given bee
    List<Transform> GetNearbyBees(FlockAgent bee)
    {
        List<Transform> nearbyBees = new List<Transform>();
        // Returns the colliders of each bee within radius
        Collider2D[] beeColliders = Physics2D.OverlapCircleAll(bee.transform.position, neighbourRadius, layerMask);
        // For each bee that has been colided with, if not current bee add to nearby bee list
        foreach(Collider2D beeCollision in beeColliders)
        {
            if (beeCollision != bee.GetBeeCollider())
            {
                nearbyBees.Add(beeCollision.transform);
            }
        }
        return nearbyBees;
    }
}
