using UnityEngine;

public class PlatformerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D player;
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float rayLength;
    
    bool moveLeft = false;
    bool moveRight = false;
    bool jumping = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) //If player presses A, set moveLeft to true, else set to false
        {
            moveLeft = true;
        }
        else
        {
            moveLeft = false;
        }

        if (Input.GetKey(KeyCode.D)) //If player presses D, set moveRight to true, else set to false
        {
            moveRight = true;
        }
        else
        {
            moveRight = false;
        }

        if (Input.GetKey(KeyCode.Space)) //If player presses space, set jumping to true, else set to false
        {
            jumping = true;
        }
        else
        {
            jumping = false;
        }
    }
    
    void FixedUpdate()
    {
        // Velocity based movement on platformer
        Vector2 velocity = player.velocity;
        RaycastHit2D hit;

        if (moveLeft)
        {
            hit = Physics2D.Raycast(player.position, Vector3.left, rayLength, layerMask); //Create raycast facing left and return hit information
            if (hit.collider == null) //If hit is null, move left (Player is not colliding with the ground)
            {
                velocity.x = -speed;
                player.transform.eulerAngles = new Vector2(0, 180); //Flip the character on its x axis to face left
            }
        }

        if (moveRight)
        {
            hit = Physics2D.Raycast(player.position, Vector3.right, rayLength, layerMask); //Create raycast facing right and return hit information
            if (hit.collider == null) //If hit is null, move right (Player is not colliding with the ground)
            {
                velocity.x = speed;
                player.transform.eulerAngles = new Vector2(0, 0); //Flip the character on its x axis to face left
            }
        }

        if (IsGrounded() && jumping) //If player on the ground and space has been pressed, jump
        {

            velocity.y = jumpForce;

        }
        player.velocity = velocity;
    }

    private bool IsGrounded()
    {
        bool isGrounded = false; //Player is not grounded by default

        RaycastHit2D hit = Physics2D.Raycast(player.position, Vector3.down, rayLength, layerMask); //Create raycast facing down and return 

        if (hit.collider != null) //If hit is not null, player is on the ground, set isGrounded to true
        {
            isGrounded = true;
            return isGrounded;
        }
        else //Else, isGrounded remains false
        {
            return isGrounded;
        }
    }
}