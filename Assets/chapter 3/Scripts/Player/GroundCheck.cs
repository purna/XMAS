using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public GameObject GroundObject;
    public bool Grounded;

    private PlayerMovement player;
    private PlayerSFX      playerSFX;
    private CameraFX       cameraFX;


    void Awake()
    {
        //Assign Components
        player    = GetComponentInParent<PlayerMovement>();
        playerSFX = FindAnyObjectByType<PlayerSFX>();
        cameraFX  = FindAnyObjectByType<CameraFX>();
    }


    public bool CheckGround()
    {
        if(GroundObject != null) return true;
        else return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.gameObject) return;
        player.SetGrounded(true);
        Grounded = true;

        if(GroundObject == null)
        {
            if(player.JumpBuffer > 0) player.Jump();
            else 
            {
                player.HasJumped = false;
                if(player.slopeAngle <= 45)
                {
                    player.rb.linearVelocity = player.CorrectMovement*player._maxSpeed;
                    cameraFX.ImpulseSource.GenerateImpulseWithForce(Math.Clamp(player.SmoothVelocity.y, -22, 0) * (cameraFX.ImpulseSource.enabled ? 1 : 0));
                }
            }
        }
        
        GroundObject = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player.gameObject) return;
        player.SetGrounded(false);
        GroundObject = null;
        Grounded = false;

        player.CoyoteTime = 0.3f;
        player._maxSpeed = Math.Clamp(player.VelocityXZ.magnitude, 5, player._maxSpeed);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player.gameObject) return;
        player.SetGrounded(true);
        GroundObject = other.gameObject;
        Grounded = true;
    }
}