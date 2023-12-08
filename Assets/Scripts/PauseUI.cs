using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    private bool pauseMenuActive = false;

    private void Update()
    {
        // If player presses escape and the pause menu is not currently active, pause game and display pause menu
        if(Input.GetKeyDown(KeyCode.Escape) && !pauseMenuActive)
        {
            Time.timeScale = 0;
            transform.GetChild(0).gameObject.SetActive(true);
            pauseMenuActive = true;
        // Close pause menu
        } else if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuActive)
        {
            ResumeGame();
        }
    }

    // Closes pause menu and resumes game
    public void ResumeGame()
    {
        Time.timeScale = 1;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        pauseMenuActive = false;
    }

    // Change pause menu to settings menu
    public void LoadSettings()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Change settings menu to pause menu
    public void BackSettings()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    // Quit is ran when player presses quit button
    public void Quit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
