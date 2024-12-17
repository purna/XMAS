using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAfterTime : MonoBehaviour
{
    public float timeToWait = 5f;  // Time to wait before loading the scene (in seconds)
    public string sceneName = "NewScene";  // Name of the scene to load

    private float timer = 0f;

    void Update()
    {
        // Increment the timer by the time passed since the last frame
        timer += Time.deltaTime;

        // If the timer exceeds the time to wait, load the scene
        if (timer >= timeToWait)
        {
            LoadScene();
        }
    }

    void LoadScene()
    {
        // Load the scene by name
        SceneManager.LoadScene(sceneName);
    }
}
