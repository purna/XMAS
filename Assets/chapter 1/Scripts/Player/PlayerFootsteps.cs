using UnityEngine;
using UnityEngine.Rendering;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioClip[] footsteps;

    public float timeBetweenSteps, stepTimer;

    public AudioManager am;

    public PlayerController pc;

    public bool IsMoving()
    {
        if (pc.MovementX != 0 || pc.MovementY != 0) return true;
        else return false;
    }

    public void Update()
    {
        if (IsMoving()) stepTimer -= Time.deltaTime;
        if (stepTimer <= 0)
        {
            stepTimer = timeBetweenSteps;
            am.PlayRandomSound(footsteps, 0.05f, 1, 0.1f);
        }
    }
}
