using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound  // Properties of a sound object
{
    [SerializeField]
    private string audioName;
    [SerializeField]
    private AudioClip audioClip;

    [SerializeField, Range(0f, 1f)]
    private float volume;
    [SerializeField, Range(.1f, 3f)]
    private float pitch = 1f;
    [SerializeField]
    private bool loop;

    [HideInInspector]
    public AudioSource audioSource;

    public string GetAudioName()
    {
        return audioName;
    }

    public void SetAudioName(string tempAudioName)
    {
        audioName = tempAudioName;
    }

    public AudioClip GetAudioClip()
    {
        return audioClip;
    }

    public void SetAudioClip(AudioClip tempAudioClip)
    {
        audioClip = tempAudioClip;
    }

    public float GetVolume()
    {
        return volume;
    }

    public void SetVolume(float tempVolume)
    {
        volume = tempVolume;
    }

    public float GetPitch()
    {
        return pitch;
    }

    public void SetPitch(float tempPitch)
    {
        pitch = tempPitch;
    }

    public bool GetLoop()
    {
        return loop;
    }

    public void SetLoop(bool tempLoop)
    {
        loop = tempLoop;
    }

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    public void SetAudioSource(AudioSource tempAudioSource)
    {
        audioSource = tempAudioSource;
    }

}
