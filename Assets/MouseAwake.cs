using UnityEngine;

public class MouseAwake : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);

        // Hide cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
