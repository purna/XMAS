using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioClip backgroundMusic;  // The audio clip to play
    private static AudioManager instance; // Singleton instance
    private AudioSource audioSource;    // AudioSource component to play the music

    void Awake()
    {
        // Check if there's already an instance of AudioManager
        if (instance == null)
        {
            // If no instance exists, this one becomes the instance
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            // If there is already an instance, destroy this object to avoid duplicates
            Destroy(gameObject);
        }

        // Get or add AudioSource component if it doesn't exist
        audioSource = GetComponent<AudioSource>();

        // Play the background music if it's not already playing
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.clip = backgroundMusic; // Set the audio clip
            audioSource.loop = true; // Loop the music
            audioSource.Play(); // Start playing the music
        }
    }

    // Optional: Change music dynamically between scenes
    public void ChangeMusic(AudioClip newMusic)
    {
        if (audioSource != null)
        {
            audioSource.Stop();  // Stop the current music
            audioSource.clip = newMusic; // Set the new music clip
            audioSource.Play(); // Play the new music
        }
    }

    // Optional: Stop the music
    public void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    // Optional: Adjust music volume
    public void SetMusicVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}
