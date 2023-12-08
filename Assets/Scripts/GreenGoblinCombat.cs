using UnityEngine;

public class GreenGoblinCombat : MonoBehaviour
{
    [SerializeField]
    AudioManager audioManager;
    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    GreenGoblin greenGoblin;

    // Items that the goblins can drop upon death and their drop chances
    [SerializeField]
    GameObject coin;
    [SerializeField]
    GameObject heart;
    [SerializeField]
    GameObject key;
    [SerializeField]
    GameObject blood;
    [SerializeField]
    private float randomCoinChance;
    [SerializeField]
    private float randomHeartChance;
    [SerializeField]
    private float randomKeyChance;

    // Current, base and ranges of health of goblins
    private int goblinHealth;
    private int goblinBaseHealth;
    [SerializeField]
    private int minHealth;
    [SerializeField]
    private int maxHealth;

    // Random number generator to calculate if goblin will drop any items
    private float rng;

    // Invulnerability period of goblins upon takng damage
    [SerializeField]
    private int invulnerabilityTime;
    private float invulnerabilityTimer;

    #region Getters
    public int GetGoblinHealth()
    {
        return goblinHealth;
    }

    public int GetGoblinBaseHealth()
    {
        return goblinBaseHealth;
    }

    public float GetInvulnerabilityTimer()
    {
        return invulnerabilityTimer;
    }
    #endregion

    private void Start()
    {
        goblinHealth = Random.Range(minHealth, maxHealth); // Randomise goblin health between respective range
        goblinBaseHealth = goblinHealth; // Store base health to dictate behaviour if goblin should be fleeing or not
    }

    void Update()
    {
        invulnerabilityTimer -= Time.deltaTime;

        // If goblin is no longer invulnerable, reset colour back to base colours
        if (invulnerabilityTimer <= 0)
        {
            greenGoblin.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }
    }

    // If goblin takes damage reduce health by respective amount, apply a knockback and put the goblin into an invulnerability period where the goblin cannot be hit
    // If goblin runs out of health, the goblin dies and can drop a coin, heart and/or key through random chance
    public void TakeDamage(int damageDealt)
    {
        if (invulnerabilityTimer <= 0)
        {
            invulnerabilityTimer = invulnerabilityTime;
            greenGoblin.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            goblinHealth -= damageDealt; // Reduce goblin health by damage
            Instantiate(blood, transform.position, greenGoblin.transform.rotation);

            greenGoblin.Knockback(); // Add knockback to goblin

            rng = Random.value;
            if (rng < 0.5)
            {
                audioManager.PlaySound("GoblinHit01");
            } else
            {
                audioManager.PlaySound("GoblinHit02");
            }


            if (goblinHealth <= 0) // If goblin health is equal to or lower than 0, set goblin object to false
            {
                rng = Random.value; // Generate float value between 0 and 1
                if(rng <= randomCoinChance) // Random chance to drop a coin
                {
                    Instantiate(coin, greenGoblin.transform.position, greenGoblin.transform.rotation);

                }

                if(rng <= randomHeartChance) // Random chance to drop a heart
                {
                    Instantiate(heart, greenGoblin.transform.position, greenGoblin.transform.rotation);
                }

                if(rng <= randomKeyChance && !gameManager.GetHasKey()) // Random chance to drop a key as long as player does not currently have one
                {
                    Instantiate(key, greenGoblin.transform.position, greenGoblin.transform.rotation);
                }
                gameObject.SetActive(false);
            }
        }
    }
}
