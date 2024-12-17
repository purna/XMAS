using UnityEngine;
using UnityEngine.SceneManagement;

public class deathfloor : MonoBehaviour
{
    public string scene;
  
  void OnTriggerEnter(Collider other)
  {

    if (other.gameObject.CompareTag("Player"))
    {
        SceneManager.LoadScene(scene);
    }

  }
}
