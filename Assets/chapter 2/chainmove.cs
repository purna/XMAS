using UnityEngine;

public class chainmove : MonoBehaviour
{

public float speed = 10;

void Update()
{
    // Moves the object forward at two units per second.
    transform.Translate(Vector3.forward * speed * Time.deltaTime);
}
}
