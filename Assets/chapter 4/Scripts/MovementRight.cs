using UnityEngine;

public class MoveObjectWithDistance : MonoBehaviour
{
    public int moveSpeed = 5;  // Speed at which the object moves
    public float distanceToMove = 10f;  // Distance the object will move before stopping

    private float startPositionX;  // To store the starting X position of the object

    void Start()
    {
        // Store the initial X position when the movement starts
        startPositionX = transform.position.x;
    }

    void Update()
    {
        // Calculate the distance moved from the starting position
        float distanceMoved = transform.position.x - startPositionX;

        // Check if the object has moved past the desired distance
        if (distanceMoved < distanceToMove)
        {
            // Move the object to the right as long as the distance hasn't been reached
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            // Stop moving the object once the distance is reached
            transform.Translate(Vector2.zero);
        }
    }
}
