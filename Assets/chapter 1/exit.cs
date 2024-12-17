using System.Security;
using UnityEditor.Rendering;
using UnityEditor.SearchService;
using UnityEngine;
 using UnityEngine.SceneManagement;
public class exit : MonoBehaviour
{

    private bool playerInRange = false;

    public Transform targetPosition;

    public GameObject player;

    public AudioSource scream, music, alarm;

    private void Update()
    {
        // Check if player is in range and the interact key is pressed
        if (playerInRange)
        {
            Teleport();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object has a tag "Player" (customize as needed)
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered the trigger zone.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting object is the player
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left the trigger zone.");
        }
    }

    private void Teleport()
    {
        player.transform.position = targetPosition.position;
        Debug.Log("Player position set to target position.");
        music.Stop();
        alarm.Play();
        Invoke("Scream", 1.0f);

        // Load Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    private void Scream()
    {
        scream.Play();
    }
}
