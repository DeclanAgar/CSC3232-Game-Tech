using UnityEngine;
using UnityEngine.UI;

public class PlatformerUI : MonoBehaviour
{
    [SerializeField]
    Text gravityText;

    [SerializeField]
    MoonGravityArea moonGravityArea;

    private float gravity;
    private float defaultGravity;
    
    private void Start()
    {
        gravity = Physics2D.gravity.y;
        defaultGravity = gravity;
        UpdateGravityText(gravity);
    }

    // Changing the Physics2D gravity values
    // Values have been reversed for readability
    private void Update()
    {
        gravity = Physics2D.gravity.y;
        if (!moonGravityArea.GetInGravityArea()) // If not in moon area, gravity can be changed
        {
            if (Input.GetMouseButton(0) && Physics2D.gravity.y >= -90) // Increase gravity
            {
                gravity -= 0.01f;
                Physics2D.gravity = new Vector2(0, gravity);
                UpdateGravityText(gravity);
            }

            if (Input.GetMouseButton(1) && Physics2D.gravity.y <= 30) // Decrease gravity
            {
                gravity += 0.01f;
                Physics2D.gravity = new Vector2(0, gravity);
                UpdateGravityText(gravity);
            }

            if (Input.GetMouseButton(2)) // Reset gravity to default
            {
                gravity = defaultGravity;
                Physics2D.gravity = new Vector2(0, gravity);
                UpdateGravityText(gravity);
            }
        }
        
    }

    public void UpdateGravityText(float gravity) // Display gravity as positive number for readability
    {
        gravityText.text = (-gravity).ToString("F2");
    }
}
