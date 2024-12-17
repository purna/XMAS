using System;
using PalexUtilities;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
        [HorizontalGroup("Speed", 0.85f)]
    public float Speed               = 50;
        [HorizontalGroup("Speed")]
        [DisplayAsString]
        [HideLabel]
    public float _speed;

        [HorizontalGroup("MaxSpeed", 0.85f)]
    public float MaxSpeed            = 80;
        [HorizontalGroup("MaxSpeed")]
        [DisplayAsString]
        [HideLabel]
    public float _maxSpeed;
    public float CounterMovement     = 10;

[Space(10)]

    public float JumpForce           = 8;
    public float Gravity             = 100;
        [Range(0,1)]
    public float SlopeStick          = 1;


    [Header("Extras")]
[PropertySpace(SpaceAfter = 10, SpaceBefore = 0)]
    [Range(0,1)] public float WalkingTime;

    [FoldoutGroup("States")]  public bool  CanMove        = true;
[Space(5)]
    [FoldoutGroup("States")]  public bool  Walking        = false;
    [FoldoutGroup("States")]  public bool  Running        = false;
    [FoldoutGroup("States")]  public bool  Crouching      = false;
    [FoldoutGroup("States")]  public bool  Paused         = false;
[Space(5)]
    [FoldoutGroup("States")]  public bool  Grounded       = true;
    [FoldoutGroup("States")]  public bool  HasJumped      = false;
    [FoldoutGroup("States")]  public bool  HoldingCrouch  = false;
    [FoldoutGroup("States")]  public bool  HoldingRun     = false;


    #region Debug Stats
        [FoldoutGroup("Debug Stats")]  public Vector3     PlayerVelocity;
        [FoldoutGroup("Debug Stats")]  public Vector3     SmoothVelocity;
        [FoldoutGroup("Debug Stats")]  public float       VelocityMagnitude;
        [FoldoutGroup("Debug Stats")]  public float       ForwardVelocityMagnitude;
        [FoldoutGroup("Debug Stats")]  public Vector3     VelocityXZ;
        [Space(5)]
        [FoldoutGroup("Debug Stats")]  public float slopeAngle;
        [FoldoutGroup("Debug Stats")]  public Vector3 slopeVector;
        [FoldoutGroup("Debug Stats")]  public Vector3 slopeCorrectionVector;
        [Space(5)]
        [FoldoutGroup("Debug Stats")]  public Vector3 CamF;
        [FoldoutGroup("Debug Stats")]  public Vector3 CamR;
        [Space(5)]
        [FoldoutGroup("Debug Stats")]  public Vector3 Movement;
        [FoldoutGroup("Debug Stats")]  public float   MovementX;
        [FoldoutGroup("Debug Stats")]  public float   MovementY;
        [FoldoutGroup("Debug Stats")]  public Vector3 CorrectMovement;
        [Space(8)]
        [FoldoutGroup("Debug Stats")]  public float CoyoteTime;
        [FoldoutGroup("Debug Stats")]  public float JumpBuffer;
        [Space(8)]
        
        [HideInInspector]
        public float   _counterMovement;
    #endregion
    
    
    #region Script / Component Reference
        [HideInInspector] public Rigidbody    rb;
        [HideInInspector] public Transform    Camera;

        private PlayerStats  playerStats;
        private PlayerSFX    playerSFX;
        private GroundCheck  groundCheck;
    #endregion


    void Awake()
    {
        //Assign Components
        rb      = GetComponent<Rigidbody>();
        Camera  = GameObject.Find("Main Camera").transform;

        //Assign Scripts
        //playerSFX    = FindAnyObjectByType<PlayerSFX>();
        playerStats  = GetComponent<PlayerStats>();
        groundCheck  = GetComponentInChildren<GroundCheck>();

        //Component Values
        rb.useGravity = false;

        //Property Values
        _maxSpeed  = MaxSpeed;
        _speed     = Speed;
    }


    void Update()
    {
        if(CoyoteTime > 0) CoyoteTime = Math.Clamp(CoyoteTime - Time.deltaTime, 0, 100);
        if(JumpBuffer > 0) JumpBuffer = Math.Clamp(JumpBuffer - Time.deltaTime, 0, 100);

        WalkingTime = Math.Clamp(WalkingTime + (Walking ? Time.deltaTime*8 : -Time.deltaTime*8), 0, 1);
    }

    void FixedUpdate()
    {
        #region PerFrame stuff
            #region Camera Orientation Values
                CamF = Camera.forward;
                CamR = Camera.right;
                CamF.y = 0;
                CamR.y = 0;
                CamF = CamF.normalized;
                CamR = CamR.normalized;

                //Rigidbody Velocity Magnitude on the X/Z Axis
                VelocityXZ = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

                // Calculate the Forward velocity magnitude
                Vector3 ForwardVelocity = Vector3.Project(rb.linearVelocity, CamF);
                ForwardVelocityMagnitude = ForwardVelocity.magnitude;
                ForwardVelocityMagnitude = (float)Math.Round(ForwardVelocityMagnitude, 2);

                WalkingCheck();
            #endregion

            SmoothVelocity = Vector3.Slerp(SmoothVelocity, rb.linearVelocity, 0.15f);
            SmoothVelocity.x    = (float)Math.Round(SmoothVelocity.x, 4);
            SmoothVelocity.y    = (float)Math.Round(SmoothVelocity.y, 4);
            SmoothVelocity.z    = (float)Math.Round(SmoothVelocity.z, 4);

            //Gravity
            rb.AddForce(Physics.gravity * Gravity /10);

            // Calculate the Forward Angle
            float targetAngle = Mathf.Atan2(rb.linearVelocity.x, rb.linearVelocity.z) * Mathf.Rad2Deg;
            Quaternion toRotation = Quaternion.Euler(0f, targetAngle, 0f);

            LockToMaxSpeed();
        #endregion
        //**********************************

        Running = HoldingRun;
        
        // Movement Code
        if(!Paused && !playerStats.Dead && CanMove)
        {
            Movement = (CamF * MovementY + CamR * MovementX).normalized;
            CalculateCorrectiveMovement();

            rb.AddForce(CorrectMovement * CalculateMoveSpeed());             // Movement
            if(Grounded) rb.AddForce(VelocityXZ * -(CounterMovement / 10));  // CounterMovement
        }

        CalculateSlopeStickForce();


        if(VelocityXZ.magnitude > 0.2)  transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 0.9f);

        #region Rounding Values
            PlayerVelocity      = rb.linearVelocity;
            PlayerVelocity.x    = (float)Math.Round(PlayerVelocity.x, 2);
            PlayerVelocity.y    = (float)Math.Round(PlayerVelocity.y, 2);
            PlayerVelocity.z    = (float)Math.Round(PlayerVelocity.z, 2);
            VelocityMagnitude   = (float)Math.Round(rb.linearVelocity.magnitude, 2);
        #endregion
    }

    //***********************************************************************
    //***********************************************************************
    //Movement Functions
    public void OnMove(InputAction.CallbackContext MovementValue)
    {
        if(Paused) return;
        Vector2 inputVector = MovementValue.ReadValue<Vector2>();
        MovementX = inputVector.x;
        MovementY = inputVector.y;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(Paused || !CanMove) return;
        if(context.started && !playerStats.Dead)
        {
            if((Grounded || CoyoteTime > 0) && !HasJumped) Jump();
            else if(!Grounded || CoyoteTime == 0) JumpBuffer = 0.15f;
        }
    }
    public void Jump()
    {
        HasJumped = true;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, math.clamp(rb.linearVelocity.y, 0, 1), rb.linearVelocity.z);
        rb.AddForce(Vector3.up * JumpForce, ForceMode.VelocityChange);

        //playerSFX.PlayRandomSound(playerSFX)
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(Paused) return;

        if(context.started)
        {
            HoldingRun = true;
        }
        if(context.canceled) 
        {
            HoldingRun = false;
        }
    }

    //***********************************************************************
    //***********************************************************************
    //Extra MoveTech

    


    //***********************************************************************
    //***********************************************************************
    //Calculations

    public float CalculateMoveSpeed()
    {
        if(Grounded)
        {
            float speedValue = Speed;

            speedValue *= Running   ? 1.5f : 1;  // Run Boost

            speedValue *= WalkingTime;        // Accelerate Hinder
            speedValue *= Crouching ? 0 : 1;  // Crouch Hinder

            _speed = speedValue;
            return speedValue;
        }

        _speed = Speed/2 * WalkingTime;
        return _speed;
    }


    float CalculateMaxSpeed()
    {
        float maxspeedValue = _maxSpeed;
        if(Grounded) _maxSpeed = MaxSpeed;

        return _maxSpeed;
    }


    private void CalculateCorrectiveMovement()
    {
        // Slope Correction
        CorrectMovement = Movement;
        if (Physics.Raycast(transform.position + (Vector3.down * 0.9f), Vector3.down, out RaycastHit hit, 1f))
        {
            if (Vector3.Angle(hit.normal, Vector3.up) <= 45 && !HasJumped)
            {
                Vector3 slopeDirection = Vector3.ProjectOnPlane(Movement, hit.normal).normalized;
                CorrectMovement = new Vector3(slopeDirection.x, Mathf.Clamp(slopeDirection.y, -0.5f, 0.1f), slopeDirection.z);

                Tools.DrawThickRay(hit.point, rb.linearVelocity, Color.red, 0, 0.001f);
                Tools.DrawThickRay(hit.point, CorrectMovement*10, Color.green, 0, 0.001f);
            }
        }
    }


    private void CalculateSlopeStickForce()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + (Vector3.down * 0.9f), Vector3.down, out hit, 1.5f))
        {
            slopeVector = hit.normal;
            slopeAngle = Vector3.Angle(slopeVector, Vector3.up);

            if (slopeAngle > 0 && slopeAngle <= 45 && !HasJumped)
            {
                // Calculate the necessary force to counteract gravity along the slope
                Vector3 slopeCorrectionVector = Vector3.ProjectOnPlane(Physics.gravity, slopeVector);

                // Apply the slope correction force
                rb.AddForce(-slopeCorrectionVector* (SlopeStick*4), ForceMode.Acceleration);
            }
        }
        // Overshoot Up correction
        if (!Grounded && !HasJumped && rb.linearVelocity.y > 0 && slopeAngle < 45)
        {
            if (Physics.Raycast(transform.position, Vector3.down, 1f))
                rb.AddForce(Vector3.down * 200, ForceMode.Acceleration);
        }

        // Overshoot Over correction
        if (!Grounded && !HasJumped && rb.linearVelocity.y < 0 && slopeAngle < 45)
        {
            if (Physics.Raycast(transform.position, Vector3.down*10, 5f))
                rb.AddForce(Vector3.down * 40, ForceMode.Acceleration);
        }
    }



    //***********************************************************************
    //***********************************************************************
    //Extra Logic

    public void Pause(bool State)
    {
        Paused = State;
        CanMove = !State;

        if(State)
        {
            MovementX = 0;
            MovementY = 0;
        }
    }

    public void LockToMaxSpeed()
    {
        // Get the velocity direction
        Vector3 newVelocity = rb.linearVelocity;
        newVelocity.y = 0f;
        newVelocity = Vector3.ClampMagnitude(newVelocity, CalculateMaxSpeed());
        newVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = newVelocity;
    }


    public void SetGrounded(bool state) 
    {
        Grounded = state;
    }

    public bool WalkingCheck()
    {
        if(MovementX != 0 || MovementY != 0)
        {
            Walking = true;
            if(Grounded) return true;
            else return false;
        }
        else
        {
            Walking = false;
            return false;
        }
    }

    [Button(DisplayParameters = true, Style = ButtonStyle.FoldoutButton)]
    public void Teleport(Transform newTransform)
    {
        rb.position = newTransform.position;
        CinemachineCamera cinemachine = FindAnyObjectByType<CinemachineCamera>();
        CinemachinePanTilt pov = cinemachine.GetComponent<CinemachinePanTilt>();

        pov.TiltAxis.Value = newTransform.eulerAngles.x;
        pov.PanAxis.Value  = newTransform.eulerAngles.y;
    }
}
