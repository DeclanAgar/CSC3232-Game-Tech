using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private int damage;
    [SerializeField]
    private Grid grid;
    
    private float rng; // Random number generator to decide chance of critical hits
    private float baseCritChance; // Base probability of a critical hit
    private int headshotDamageModifier; // Added damage value if critical hit occurs
    public static float currentRandomCritChance;

    private void Start()
    {
        baseCritChance = 0.10f;
        currentRandomCritChance = Data.randomCritChance;
        headshotDamageModifier = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If collision is a destructable and has been hit by hammer, destroy object
        if (collision.CompareTag("Destructable") && gameObject.name == "HeavyHammer")
        {
            Destroy(collision.gameObject);
            grid.SetMapChanged(true); // Map has changed, recalculate map
        }

        if (collision.CompareTag("Enemy") && collision.GetComponent<GreenGoblinCombat>().GetInvulnerabilityTimer() <= 0)
        { // If weapon collides with enemy, deal damage to enemy
            
            rng = Random.value; // Generate random float number from 0 to 1 as chance to crit

            // Play weapon sounds
            if (gameObject.name == "LightSword")
            {
                audioManager.PlaySound("SwordHit01");
            } else
            {
                if (rng < 0.5)
                {
                    audioManager.PlaySound("HammerHit01");
                } else
                {
                    audioManager.PlaySound("HammerHit02");
                }
            }
            if (rng <= Data.randomCritChance) // Player has crit, deal double damage and reset crit chance
            {
                Data.randomCritChance = baseCritChance; 
                collision.GetComponent<GreenGoblinCombat>().TakeDamage((damage * 2) + CheckHeadshot(collision));
            }
            else // Player hasn't crit, deal base damage and slightly increase crit chance
            {
                Data.randomCritChance += 0.05f;
                collision.GetComponent<GreenGoblinCombat>().TakeDamage(damage + CheckHeadshot(collision));
            }
        }
    }

    // Check the collision of the specific hit area (head) of goblin. If weapon has hit head, deal added damage
    // A headshot can only occur if the weapon hits ONLY the head. This is to make headshots occur less often
    private int CheckHeadshot(Collider2D collision)
    {
        if (collision.GetComponent<CircleCollider2D>().IsTouching(gameObject.GetComponent<BoxCollider2D>()) && !collision.GetComponent<BoxCollider2D>().IsTouching(gameObject.GetComponent<BoxCollider2D>()))
        { 
            return headshotDamageModifier;
        }
        return 0;
    }
}