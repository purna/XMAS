using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFX : MonoBehaviour
{
    public GameObject lookingTarget;
    public GameObject lookingTargetStorage;
    public Camera cam;

    public void Awake()
    {
        cam = Camera.main;
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
    if(context.started)
    {
        if(lookingTarget == null) return;
        Interactable newItem = lookingTarget.GetComponent<Interactable>();
        if (newItem != null) newItem.InteractStart();
    }
    if(context.canceled)
    {
        if(lookingTarget == null) return;
        Interactable newItem = lookingTarget.GetComponent<Interactable>();
        if (newItem != null) newItem.InteractEnd();
    }
    }
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 3))
        {
            // If hit something, assign lookingTarget to the hit object
            lookingTarget = hit.collider != null ? hit.collider.gameObject : null;
        }
        else
        {
            // If hit nothing, clear the target
            lookingTarget = null;
        }
        if (lookingTarget != null) // We hit an object
        {
            if (lookingTarget != lookingTargetStorage) // New/Unique Object hit
            {
                // Call MouseOver() on the new interactable object
                Interactable newItem = lookingTarget.GetComponentInParent<Interactable>();
                if (newItem != null) newItem.MouseOver();
                // If there was a previous object, call MouseExit() on it
                Interactable oldItem = lookingTargetStorage != null ? lookingTargetStorage.GetComponentInParent<Interactable>() : null;
                if (oldItem != null) oldItem.MouseExit();
                // Update the storage to the current object
                lookingTargetStorage = lookingTarget;
            }
        }
        else // We are not hitting any object
        {
            // If there was a previous object, call MouseExit() on it
            Interactable oldItem = lookingTargetStorage != null ? lookingTargetStorage.GetComponentInParent<Interactable>() : null;
            if (oldItem != null) 
            {
                oldItem.MouseExit();
                oldItem.InteractEnd();
            }
            // Clear the storage since nothing is being looked at
            lookingTargetStorage = null;
        }
    }
}
