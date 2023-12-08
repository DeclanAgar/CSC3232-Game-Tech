using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    // Values to be retained over scenes. This is to ensure that actions from player in previous scenes
    // benefit them in future ones
    public static int playerHealth = 3;
    public static int score = 0;
    public static float randomCritChance = 0.1f;
    public static float heavyAttackCooldown = 6f;

    public static void ResetValues() // To be ran if user accesses levels from level select
    {
        playerHealth = 3;
        score = 0;
        randomCritChance = 0.1f;
        heavyAttackCooldown = 6f;
    }
}
