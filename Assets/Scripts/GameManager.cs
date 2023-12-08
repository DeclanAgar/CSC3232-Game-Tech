using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private Door door;

    // Gameobjects in  UI to be enabled/disabled upon the end of a level
    [SerializeField]
    private GameObject levelEnd;
    [SerializeField]
    private GameObject complete;
    [SerializeField]
    private GameObject failed;

    // Time delay until a scene is restarted/loaded
    [SerializeField]
    float delay;

    private bool hasKey = false;

    #region Getters and Setters
    public bool GetHasKey()
    {
        return hasKey;
    }

    public void SetHasKey(bool tempHasKey)
    {
        hasKey = tempHasKey;
    }
    #endregion

    private void Update()
    {
        // If in a level, user presses return and the player is overlapping a door, complete level
        if (SceneManager.GetActiveScene().name != "LevelSelect" && Input.GetKeyDown(KeyCode.Return) && door.GetPlayerOverlapping())
        {
            audioManager.PlaySound("FinishLevel");
            CompleteLevel();
        }
    }

    // Display level completed on screen and load next level after set delay
    public void CompleteLevel()
    {
        levelEnd.SetActive(true);
        complete.SetActive(true);
        failed.SetActive(false);

        Invoke("LoadNextLevel", delay);
    }

    // Load next scene on build index
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Display level failed on screen, reset values to values at start of current level and load same level after set delay
    public void EndGame()
    {
        levelEnd.SetActive(true);
        complete.SetActive(false);
        failed.SetActive(true);

        // Reset values to start of level
        Data.playerHealth = PlayerCollision.currentPlayerHealth;
        Data.score = PlayerCollision.currentScore;
        Data.randomCritChance = WeaponCollision.currentRandomCritChance;
        Data.heavyAttackCooldown = PlayerCombat.currentHeavyAttackCooldown;
        Invoke("Restart", delay);
    }

    // Load current scene
    void Restart()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
