using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip backgroundMusic; // Audio clip for background music
    private static MusicManager instance; // Singleton instance
    private AudioSource audioSource; // AudioSource component to play the music

    void Awake()
    {
        // Check if there is already an instance of MusicManager
        if (instance == null)
        {
            // If no instance exists, this one becomes the instance
            instance = this;
            DontDestroyOnLoad(gameObject); // Don't destroy this object on scene load
        }
        else
        {
            // If there is already an instance, destroy this object to prevent duplicates
            Destroy(gameObject);
            return; // Exit to prevent the rest of the method from running
        }

        // Add an AudioSource component if not already attached
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource if missing
        }

        // Start playing the background music if not already playing
        if (!audioSource.isPlaying)
        {
            audioSource.clip = backgroundMusic; // Set the audio clip
            audioSource.loop = true; // Loop the music
            audioSource.Play(); // Play the music
        }
    }
}