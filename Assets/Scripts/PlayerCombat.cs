using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    Rigidbody2D player;
    [SerializeField]
    private UserInterface ui;
    

    [SerializeField]
    GameObject lightSword;
    [SerializeField]
    GameObject heavyHammer;
    [SerializeField]
    GameObject lightAttackIcon;
    [SerializeField]
    GameObject heavyAttackIcon;
    [SerializeField]
    GameObject blood;

    // How long player will be in attack for
    private float attackDuration;

    [SerializeField]
    private float lightAttackCooldown;

    public static float currentHeavyAttackCooldown;

    // Differentiate whether player is light or heavy attacking
    private bool lightAttacking;
    private bool heavyAttacking;

    [SerializeField]
    private float invulnerabilityTime;    

    // Int variables to be manipulated by animation
    private int facingUp;
    private int facingDown;
    private int facingLeft;
    private int facingRight;

    // Fix position of hammer when attacking
    private bool positionFixed = false;

    // Stores coordinate to dictate position and rotation of weapons
    private Vector3 direction;
    private Vector3 offset;

    // Timers to control how long player can be attacking and invulnerable
    private float attackTimer;
    private float invulnerabilityTimer;

    #region Getters and Setters
    public void SetFacingUp(int tempFacingUp)
    {
        facingUp = tempFacingUp;
    }

    public void SetFacingDown(int tempFacingDown)
    {
        facingDown = tempFacingDown;
    }

    public void SetFacingLeft(int tempFacingLeft)
    {
        facingLeft = tempFacingLeft;
    }

    public void SetFacingRight(int tempFacingRight)
    {
        facingRight = tempFacingRight;
    }

    public void SetLightAttackCooldown(float tempLightAttackCooldown)
    {
        lightAttackCooldown = tempLightAttackCooldown;
    }

    public float GetLightAttackCooldown()
    {
        return lightAttackCooldown;
    }

    public void SetHeavyAttackCooldown(float tempHeavyAttackCooldown)
    {
        Data.heavyAttackCooldown = tempHeavyAttackCooldown;
    }

    public float GetHeavyAttackCooldown()
    {
        return Data.heavyAttackCooldown;
    }

    #endregion

    private void Start()
    {
        attackTimer = -1f;
        invulnerabilityTimer = 0f;
        currentHeavyAttackCooldown = Data.heavyAttackCooldown;

        attackDuration = 1f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && attackTimer <= -lightAttackCooldown) // Player presses left click and light attack cooldown is over
        {
            lightAttacking = true;
            attackTimer = attackDuration; // Reset light attack attackTimer to attackDuration
            audioManager.PlaySound("WeaponSwing01");
        } else if (Input.GetMouseButtonDown(1) && attackTimer <= -Data.heavyAttackCooldown)
        {
            heavyAttacking = true;
            attackTimer = attackDuration;
            audioManager.PlaySound("WeaponSwing02");
        }

        if (attackTimer <= -Data.heavyAttackCooldown) // Player can attack with light and heavy attack
        {
            lightAttackIcon.SetActive(true);
            heavyAttackIcon.SetActive(true);
        }
        else if (attackTimer <= -lightAttackCooldown) // Player can attack with only light attack
        {
            lightAttackIcon.SetActive(true);
            heavyAttackIcon.SetActive(false);
        }
        else // Player cannot currently attack
        {
            lightAttackIcon.SetActive(false);
            heavyAttackIcon.SetActive(false);
        }

        invulnerabilityTimer -= Time.deltaTime; // Continuously decrease invulnerabilityTimer by frame time
        attackTimer -= Time.deltaTime; // Continuously decrease attackTimer by frame time

        if (invulnerabilityTimer <= 0)
        {
            player.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }

        if (attackTimer > 0)
        { // Continue attacking as long as timer is above 0
            if (lightAttacking)
            {
                // Enable light sword and disable heavy hammer
                lightSword.SetActive(true);
                heavyHammer.SetActive(false);

                if (facingUp == 1) // If player is facing up, move sword up
                {
                    offset.y = 1.25f;
                    lightSword.transform.localPosition = offset;
                }
                else if (facingLeft == 1 || facingRight == 1) // If player is facing left or right, move sword up
                {
                    offset.y = 0.5f;
                    lightSword.transform.localPosition = offset;
                }
                else // Player is facing down, sword doesn't need to move
                {
                    lightSword.transform.localPosition = new Vector3(0, 0, 0);
                }

                if (facingUp == 1) // If player is facing up, rotate sword to face up
                {
                    direction.z = 0;
                    lightSword.transform.rotation = Quaternion.Euler(direction);
                }
                else if (facingDown == 1) // If player is facing down, rotate sword to face down
                {
                    direction.z = 180;
                    lightSword.transform.rotation = Quaternion.Euler(direction);
                }
                else if (facingLeft == 1) // If player is facing left, rotate sword to face left
                {
                    direction.z = 90;
                    lightSword.transform.rotation = Quaternion.Euler(direction);
                }
                else if (facingRight == 1) // If player is facing right, rotate sword to face right
                {
                    direction.z = 270;
                    lightSword.transform.rotation = Quaternion.Euler(direction);
                }
            } else if (heavyAttacking)
            {
                // Enable heavy hammer and disable light sword
                lightSword.SetActive(false);
                heavyHammer.SetActive(true);

                if (facingUp == 1) // If player is facing up, rotate hammer to face up
                {
                    if (!positionFixed)
                    {
                        offset.y = 1.25f;
                        heavyHammer.transform.localPosition = offset;
                        direction.z = -90;
                        heavyHammer.transform.rotation = Quaternion.Euler(direction);
                        positionFixed = true; // Lock hammer position in place
                    }   
                }
                else if (facingDown == 1) // If player is facing down, rotate hammer to face down
                {
                    if (!positionFixed)
                    {
                        heavyHammer.transform.localPosition = new Vector3(0, 0, 0);
                        direction.z = 90;
                        heavyHammer.transform.rotation = Quaternion.Euler(direction);
                        positionFixed = true; // Lock hammer position in place
                    }
                }
                else if (facingLeft == 1) // If player is facing left, rotate hammer to face left
                {
                    if (!positionFixed)
                    {
                        offset.y = 0.5f;
                        heavyHammer.transform.localPosition = offset;
                        direction.z = 0;
                        heavyHammer.transform.rotation = Quaternion.Euler(direction);
                        positionFixed = true; // Lock hammer position in place
                    }
                }
                else if (facingRight == 1) // If player is facing right, rotate hammer to face right
                {
                    if (!positionFixed)
                    {
                        offset.y = 0.5f;
                        heavyHammer.transform.localPosition = offset;
                        direction.z = 180;
                        heavyHammer.transform.rotation = Quaternion.Euler(direction);
                        positionFixed = true; // Lock hammer position in place
                    }
                }
            }
        }
        else
        {
            // Player no longer attacking, reset booleans and disable weapons
            lightAttacking = false;
            lightSword.SetActive(false);
            heavyAttacking = false;
            heavyHammer.SetActive(false);
            positionFixed = false;
        }
    }

    // If player takes damage reduce health by respective amount and put the player into an invulnerabiltiy period where the player cannot be hit
    // If player runs out of health, the player dies and the level will restart
    public void TakeDamage(int damageDealt)
    {
        if (invulnerabilityTimer <= 0) // If player is no longer invulnerable
        {
            invulnerabilityTimer = invulnerabilityTime;
            player.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            Data.playerHealth -= damageDealt;
            Instantiate(blood, transform.position, transform.rotation);
            audioManager.PlaySound("PlayerHit");
            ui.UpdateHealth(); // Update health on UI

            if (Data.playerHealth <= 0) // If player health is equal to or less than 0, player has died, end game state
            {   
                gameObject.SetActive(false);
                audioManager.PlaySound("PlayerHit");
                audioManager.PlaySound("GameOver");
                FindObjectOfType<GameManager>().EndGame();  
            }
        }
    }

    // Int variables to act as booleans for animation events
    public void ResetFacings() 
    {
        facingUp = 0;
        facingDown = 0;
        facingLeft = 0;
        facingRight = 0;
    }
}
