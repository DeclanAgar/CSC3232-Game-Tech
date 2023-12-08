using UnityEngine;

public class MoonGravityArea : MonoBehaviour
{
    [SerializeField]
    PlatformerUI platformerUI;

    private float moonGravity = -4.98f; // 16.6% of -30 (mimic moon gravity)
    private float normalGravity = -30f;
    private bool inGravityArea = false;

    #region Getters
    public bool GetInGravityArea()
    {
        return inGravityArea;
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // If in moon area, set gravity to mimic moon (Set to 16.6% of current gravity)
        {
            Physics2D.gravity = new Vector2(0, moonGravity);
            platformerUI.UpdateGravityText(moonGravity);
            inGravityArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // If player leaves moon area, set gravity back to default
        {
            Physics2D.gravity = new Vector2(0, normalGravity);
            platformerUI.UpdateGravityText(normalGravity);
            inGravityArea = false;
        }
    }

}
