using System;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

// The concept of an AudioManager to enable a simple and intuitive way to play audio
// comes from 'https://youtu.be/6OT43pvUyfY' 'Introduction to AUDIO in Unity' by Brackeys

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer; 
    [SerializeField]
    private Sound[] sounds;

    
    void Awake()
    {
        // For every sound added to the sound array, add an audiosource component representing each audio clip
        foreach (Sound sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.GetAudioClip();
            sound.audioSource.volume = sound.GetVolume();
            sound.audioSource.pitch = sound.GetPitch();
            sound.audioSource.loop = sound.GetLoop();
        }
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        // If current scene is main menu or settings, play main theme
        if (SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "Settings")
        {
            PlaySound("MainTheme");

            
        } else if (SceneManager.GetActiveScene().name == "LevelSelect") { // If current scene is level select, play levelselect theme
            PlaySound("LevelSelectTheme");
        } else if (SceneManager.GetActiveScene().name == "Credits") // If current scene is credits, play credits theme
        {
            PlaySound("CreditTheme");
            
        } else
        { // Else current scene is a level, play battle theme
            PlaySound("BattleTheme");
        }
    }

    public void ChangeMasterVolume(float masterVolume)
    {
        audioMixer.SetFloat("masterVolume", masterVolume); //Set mastervolume audio mixer to given float
        foreach (Sound s in sounds)
        {
            s.audioSource.volume = (masterVolume + 80) * s.GetVolume() / 80; // Set volume of each audio source to reflect the master audio slider setting
        }
    }

    // PlaySound loops through the array of sounds and finds and plays the sound which name matches the string parameter
    public void PlaySound(string audioName)
    {
        foreach (Sound s in sounds)
        {
            if (s.GetAudioName() == audioName)
            {
                s.audioSource.Play();
                return;
            }
        }
    }
}
