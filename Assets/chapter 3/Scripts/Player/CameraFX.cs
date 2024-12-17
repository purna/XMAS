using System;
using System.Collections;
using System.Collections.Generic;
using PalexUtilities;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;


namespace chapter3{


public class CameraFX : MonoBehaviour
{
    public float TargetDutch;
    private float BlendDutch;

    public float TargetFOV;
    private float BlendFOV;

    public float TargetShakeAmplitude;
    private float BlendShakeAmplitude;
    public float TargetShakeFrequency;
    private float BlendShakeFrequency;

    [Space(10)]

    public GameObject LookingTarget;
    public GameObject LookingTargetStorage;


    [Foldout("Cinemachine stuff")]
    public Camera cam;
    public CinemachineCamera cinemachine;
    public CinemachineRecomposer recomposer;
    public CinemachineOrbitalFollow pov;
    public CinemachineBasicMultiChannelPerlin PerlinShake;
    public CinemachineImpulseSource ImpulseSource;
    public CinemachineImpulseListener ImpulseListener;
    public CinemachineInputAxisController InputAxisController;
    public PlayerMovement playerMovement;
    public PlayerStats playerStats;

    void Awake()
    {
        //Assign Components
        cam = Camera.main;

        cinemachine          = FindAnyObjectByType<CinemachineCamera>();
        pov                  = cinemachine.GetComponent<CinemachineOrbitalFollow>();
        recomposer           = cinemachine.GetComponent<CinemachineRecomposer>();
        PerlinShake          = cinemachine.GetComponent<CinemachineBasicMultiChannelPerlin>();
        ImpulseSource        = FindAnyObjectByType<CinemachineImpulseSource>();
        ImpulseListener      = GetComponent<CinemachineImpulseListener>();
        InputAxisController  = GetComponent<CinemachineInputAxisController>();

        playerMovement  = FindAnyObjectByType<PlayerMovement>();
        playerStats     = FindAnyObjectByType<PlayerStats>();
    }

    void Update()
    {
        InputAxisController.enabled = true;
        
        if(playerStats.Dead || playerMovement.Paused)
        {
            recomposer.Dutch = 0;
            recomposer.ZoomScale = 1;
            PerlinShake.AmplitudeGain = 0;
            PerlinShake.FrequencyGain = 0;
            return;
        }

        TargetFOV = 1 + Math.Abs(playerMovement.rb.linearVelocity.magnitude/100);


        //Dutch Tilt + Field Of View
        recomposer.Dutch = Mathf.SmoothDamp(recomposer.Dutch, TargetDutch, ref BlendDutch, 0.1f);
        recomposer.ZoomScale = Mathf.SmoothDamp(recomposer.ZoomScale, TargetFOV, ref BlendFOV, 0.2f);
        TargetDutch = playerMovement.MovementX * -1.5f;


        //Footstep
        PerlinShake.AmplitudeGain = Mathf.SmoothDamp(PerlinShake.AmplitudeGain, TargetShakeAmplitude, ref BlendShakeAmplitude, 0.1f);
        PerlinShake.FrequencyGain = Mathf.SmoothDamp(PerlinShake.FrequencyGain, TargetShakeFrequency, ref BlendShakeFrequency, 0.1f);

        
        //Land Force
        if(!playerMovement.Crouching) ImpulseSource.DefaultVelocity.y = 0.25f;
        else ImpulseSource.DefaultVelocity.y = 0.15f;


        //Footstep Shake Conditions
        if(playerMovement.WalkingCheck())
        {
            if(playerMovement.Running)
            {
                TargetShakeAmplitude = 4f;
                TargetShakeFrequency = 0.05f;
            }
            else if(playerMovement.Crouching)
            {
                TargetShakeAmplitude = 1f;
                TargetShakeFrequency = 0.03f;
            }
            else
            {
                TargetShakeAmplitude = 3f;
                TargetShakeFrequency = 0.04f;
            }
        }
        else
        {
            TargetShakeAmplitude = 0f;
            TargetShakeFrequency = 0f;
        } 

        // RaycastHit hit = Tools.GetCameraForwardHit3D(12);
        // LookingTarget = hit.collider == null ? null : hit.collider.gameObject;
        // if (LookingTarget != null) // Object
        // {
        //     if (LookingTarget != LookingTargetStorage) // Unique Object
        //     {
        //         //DebugPlus.DrawSphere(Tools.GetCameraForwardHit3D().point, 0.25f).Color(Color.green).Duration(0.1f);

        //         Interactable newItem = LookingTarget.GetComponentInParent<Interactable>();
        //         if (newItem != null) newItem.MouseOver();

        //         Interactable oldItem = LookingTargetStorage != null ? LookingTargetStorage.GetComponentInParent<Interactable>() : null;
        //         if (oldItem != null) oldItem.MouseExit();

        //         LookingTargetStorage = LookingTarget;
        //     }
        //     //else DebugPlus.DrawSphere(Tools.GetCameraForwardHit3D().point, 0.1f).Color(Color.red);
        // }
        // else
        // {
        //     Interactable oldItem = LookingTargetStorage != null ? LookingTargetStorage.GetComponentInParent<Interactable>() : null;
        //     if (oldItem != null) oldItem.MouseExit();
        //     LookingTargetStorage = null;
        // }
    }


    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(LookingTarget == null) return;
            Interactable newItem = LookingTarget.GetComponent<Interactable>();
            if (newItem != null) newItem.InteractStart();
        }
    }

    public void Die()
    {
        cinemachine.enabled = false;
        TargetShakeAmplitude = 0f;
        TargetShakeFrequency = 0f;
    }
}
}