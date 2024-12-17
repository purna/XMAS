using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musiccc : MonoBehaviour
{
    public AudioClip backgroundMusic;  // The audio clip to play
    private static Musiccc instance; // Singleton instance
    private AudioSource audioSource;    // AudioSource component for playing music

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

    // Optionally, you can add methods to adjust the music volume or stop music
    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
