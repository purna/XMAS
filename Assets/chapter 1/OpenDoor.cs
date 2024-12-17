using UnityEngine;

public class OpenDoor : MonoBehaviour
{

    public bool doorIsOpen;

    public float Speed;

    private Vector3 OriginPosition;
    public Vector3 OpenPosition;
    private Vector3 TargetPosition;

    private Quaternion OriginRotation;
    public Vector3 OpenRotationEuler; // Keep as Vector3 for user-friendly input.
    private Quaternion OpenRotation;
    private Quaternion TargetRotation;

    private PlayerController pc;
    private UI uI;
    
    private bool playerInRange = false;
    public bool keyCollected;
    public KeyCode interactKey = KeyCode.E; // The key to press (default: E)

    private void Start()
    {
        OriginPosition = transform.position;
        TargetPosition = OriginPosition;

        OriginRotation = transform.rotation;
        OpenRotation = Quaternion.Euler(OpenRotationEuler); // Convert to quaternion.
        TargetRotation = OriginRotation;
        pc = FindAnyObjectByType<PlayerController>();
        uI = FindAnyObjectByType<UI>();
        doorIsOpen = false;
    }

    private void Update()
    {
        transform.position = Vector3.Slerp(transform.position, TargetPosition, Speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, Speed);

        // Check if player is in range and the interact key is pressed
        if (playerInRange && Input.GetKeyDown(interactKey) && keyCollected == true)
        {
            if (!doorIsOpen) TheDoorOpens();
        }
    }

    //private void TheDoorCloses()
    //{   
    //    doorIsOpen = false;
    //    TargetPosition = OriginPosition;
    //    TargetRotation = OriginRotation;
    //}

    private void TheDoorOpens()
    {
        uI.keyCollected.SetText("");
        doorIsOpen = true;
        TargetPosition = OriginPosition + OpenPosition;
        TargetRotation = OpenRotation;
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

}
