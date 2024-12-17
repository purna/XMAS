using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public string sceneName; // The name of the scene to load

    // This method will be called when the button is clicked
    public void LoadScene()
    {
        // Load the scene by name
        SceneManager.LoadScene(sceneName);
    }
}
