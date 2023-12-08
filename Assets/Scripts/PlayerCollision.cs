using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    // Script references
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField]
    private PlayerCombat playerCombat;
    [SerializeField]
    private UserInterface ui;

    // Storing of current player values
    public static int currentPlayerHealth;
    public static int currentScore;

    [SerializeField]
    private int pointsPerCoin = 1;

    // Damage values
    [SerializeField]
    private int spikeDamage = 2;
    [SerializeField]
    private int goblinDamage = 1;

    private bool OnSpikes = false;

    private void Start()
    {
        currentPlayerHealth = Data.playerHealth;
        currentScore = Data.score;
    }

    #region Getters and Setters
    public void SetOnIceFalse()
    {
        playerMovement.SetOnIce(false);
    }

    public bool GetOnSpikes()
    {
        return OnSpikes;
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Key")) // If player collides with a key, disable key and enable escape from level
        {
            collision.gameObject.SetActive(false);
            gameManager.SetHasKey(true);
            audioManager.PlaySound("Key");
            ui.UpdateKey();
        }

        if (collision.CompareTag("Heart")) // If player collides with a heart, disable heart and increase their health by 1
        {
            collision.gameObject.SetActive(false);
            Data.playerHealth += 1;
            audioManager.PlaySound("Heart");
            ui.UpdateHealth();
        } 

        if(collision.CompareTag("Collectable")) // If player collides with a coin, disable coin and increase score
        {
            collision.gameObject.SetActive(false);
            Data.score += pointsPerCoin;
            audioManager.PlaySound("Coin");
            ui.UpdateScore();

            // Positive feedback loops
            // For every 10 score gained by a player, increase their health by 1
            if(Data.score % 10 == 0)
            {
                Data.playerHealth += 1;
                audioManager.PlaySound("Heart");
                ui.UpdateHealth();
            }
            
            // For every 5 score gained by a player, reduce heavy attack cooldown by 5% as long as it is not less than 3 seconds
            if (Data.score % 5 == 0 && playerCombat.GetHeavyAttackCooldown() > 3.0f)
            {
                float cooldownDecrease = playerCombat.GetHeavyAttackCooldown() * 0.95f;
                playerCombat.SetHeavyAttackCooldown(cooldownDecrease);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ice")) // If player is in an ice zone, set on ice value to true
        {
            playerMovement.SetOnIce(true);
        }

        if (collision.CompareTag("DeathZone")) // If player collides with deathzone, take damage
        {
            OnSpikes = true;
            playerCombat.TakeDamage(spikeDamage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ice")) // If player leaves an ice zone, set on ice value to false
        {
            Invoke("SetOnIceFalse", 0.5f);
        }

        if (collision.CompareTag("DeathZone")) // Player is no longer oon deathzone, set on spikes to false
        {
            OnSpikes = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy")) // If player collides with enemy, take damage
        {
            playerCombat.TakeDamage(goblinDamage);

        }
    }
}