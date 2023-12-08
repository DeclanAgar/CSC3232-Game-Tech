using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D player;

    [SerializeField]
    private Animator animator;

    // Movement based variables 
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    float moveForce = 500f;
    private Vector2 movement;
    public bool onIce;

    #region Getters and Setters
    public Vector2 GetMovement()
    {
        return movement;
    }
    public void SetMovement(Vector2 tempMovement)
    {
        movement = tempMovement;
    }

    public void SetOnIce(bool tempOnIce)
    {
        onIce = tempOnIce;
    }

    public void SetOnIceFalse()
    {
        onIce = false;
    }

    

    #endregion

    // Update is called once per frame
    void Update()
    {
        // Set x and y values based on horizontal and vertical user input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Insert x and y values to be used by blend tree to dictate which animation to use
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);

        // If enemy is moving, set moving to true and run blend tree
        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else // Else enemy is not moving do not run blend tree animations
        {
            animator.SetBool("isMoving", false);
        }    
    }

    void FixedUpdate()
    {
        if (!onIce)
        {
            // Not on ice, use moveposition method based movement
            player.velocity = Vector2.zero;
            player.MovePosition(player.position + movement.normalized * speed * Time.fixedDeltaTime);
        }
        else
        { // On ice, use force based movement
            player.AddForce(movement.normalized * moveForce * Time.fixedDeltaTime);
        }
    }
}
