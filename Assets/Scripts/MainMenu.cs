using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("LevelSelect");//Load level selection scene
    }

    public void LoadSettings()
    {
        SceneManager.LoadScene("Settings");//Load Settings menu
    }

    public void Quit()
    {
        Debug.Log("Quitting");
        Application.Quit();//Close game if in application
    }
}

