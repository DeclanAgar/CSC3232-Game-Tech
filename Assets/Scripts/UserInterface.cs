using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    // Text and images to update
    [SerializeField]
    Text fpsText;
    [SerializeField]
    Text healthText;
    [SerializeField]
    Text scoreText;
    
    [SerializeField]
    Image key;

    private float timer;

    private void Awake()
    {
        UpdateScore();
        UpdateHealth();
    }

    private void Update()
    {
        timer += Time.unscaledDeltaTime;

        // Update framerate every half second
        if (timer > 0.5)
        {
            timer = 0;
            fpsText.text = (1 / Time.unscaledDeltaTime).ToString("0"); // 1 / time since last frame gives amount of frames per second
        }
    }

    // Update score on UI
    public void UpdateScore()
    {
        scoreText.text = Data.score.ToString("0"); //Set score to players current score value
    }

    // Update health on UI
    public void UpdateHealth()
    {
        healthText.text = Data.playerHealth.ToString("0"); //Set health to players current health
    }

    // Update key on UI 
    public void UpdateKey()
    { 
        key.color = new Color(255, 255, 255); // Display correct colour of key to indicate that it has been collected
    }
}
