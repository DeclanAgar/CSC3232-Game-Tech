using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private AudioManager audioManager;

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu"); //Go back to main menu
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen; //Toggle fullscreen
    }

    public void SetVolume(float volume)
    {
        audioManager.ChangeMasterVolume(volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex); //Set quality level to chosen index
    }

    public void ButtonAudio(string audioName)
    {
        FindObjectOfType<AudioManager>().PlaySound(audioName);
    }
}
