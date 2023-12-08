using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockBehaviour : MonoBehaviour
{
    [SerializeField]
    float separationThreshold;

    // Alignment aligns bees to the average direction of nearby bees
    public Vector2 Alignment(FlockAgent bee, List<Transform> nearbyBees)
    {
        // If no nearby bees, keep facing same direction
        if (nearbyBees.Count == 0)
        {
            return bee.transform.up;
        }

        Vector2 direction = bee.transform.up;

        // For earh nearby bee, add their direction
        foreach(Transform nearbyBee in nearbyBees)
        {
            direction += (Vector2)nearbyBee.transform.up;
        }
        // Get average direction
        direction /= nearbyBees.Count;

        return direction;                
    }

    // Separation separates bees from nearby bees and scales separation strength based on proximity
    public Vector2 Separation(FlockAgent bee, List<Transform> nearbyBees)
    {
        // If no nearby bees,do not add a separation value
        if (nearbyBees.Count == 0)
        {
            return Vector2.zero;
        }

        Vector2 direction = new Vector2();

        // For each nearby bee, calculate distance between bee and nearby bee and if within range add a separation value to direction
        foreach(Transform nearbyBee in nearbyBees)
        {
            float distance = Vector2.Distance(bee.transform.position.normalized, nearbyBee.position.normalized);
            if (distance > separationThreshold)
            {
                continue;
            }
            float strength = 1.0f - (distance / separationThreshold); // Strength scales based on proximity of nearby bee
            direction += (Vector2)(bee.transform.position - nearbyBee.position).normalized * strength;
        }
        return direction.normalized;
    }

    // Cohesion groups nearby bees together to act as a single entity
    public Vector2 Cohesion(FlockAgent bee, List<Transform> nearbyBees)
    {
        // If no bees, do not add a cohesion value
        if (nearbyBees.Count == 0)
        {
            return Vector2.zero;
        }

        Vector2 averagePosition = Vector2.zero;

        // For each nearby bee, get position and add to average position vector
        foreach(Transform nearbyBee in nearbyBees)
        {
            averagePosition += (Vector2)nearbyBee.position;
        }
        // Average all nearby bees position
        averagePosition /= nearbyBees.Count;
        Vector2 direction = averagePosition - (Vector2)bee.transform.position; // Convert world position to local position

        return direction.normalized;
    }
}
