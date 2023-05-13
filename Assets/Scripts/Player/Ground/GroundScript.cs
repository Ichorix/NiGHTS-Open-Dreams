using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundScript : MonoBehaviour
{
    public GroundAnimationController animationController;
    public StateController stateController;
    public SurfaceAligning surfaceAligning;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float movementSpeed, rotationSpeed, gTime, jumpPower, switchCoolDown, speedBuffTime;
    [SerializeField] private bool switchCoolDownOver, speedBuff;
    public Quaternion surfaceAlignmentRot, totalRot;
    public Vector3 movementToCameraRot;

    public float gravityMult;

    public float rotatingVertical, rotatingHorizontal;
    void Start()
    {
        //animationController = GameObject.Find("NightsGroundModel").GetComponent<GroundAnimationController>();
        //stateController = GameObject.Find("PlayerStates").GetComponent<StateController>();

        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityMult;
    }
    void OnEnable()
    {
        Debug.Log("Was enabled");
        switchCoolDown = 0;
        switchCoolDownOver = false;
    }
    void Update()
    {
        switchCoolDown += Time.deltaTime;
        if(switchCoolDown >= 1.5f)
        {
            switchCoolDownOver = true;
        }

        if(speedBuff)
        {
            speedBuffTime += Time.deltaTime;
        }
        if(speedBuffTime >= 10)
        {
            movementSpeed = 75;
            speedBuff = false;
            speedBuffTime = 0;
        }
        //totalRot.x = surfaceAlignmentRot.x;
        //totalRot.y = movementToCameraRot.y;
        //totalRot.z = surfaceAlignmentRot.z;

        //transform.rotation = totalRot;
    }
    void FixedUpdate()
    {
        MovementRelativeToCamera();
    }
    public void Moving(InputAction.CallbackContext context)// Jumping Control
    {
        Debug.Log("Moving" + context);
        if(context.started && switchCoolDownOver == true)
        {
            playerRb.AddForce(Vector3.up * jumpPower);
            animationController.JumpingAnimation();
            stateController.HasJumped();
        }   
    }
    public void Rotating(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        rotatingVertical = direction.x;
        rotatingHorizontal = direction.y;
        Debug.Log(rotatingVertical + "," + rotatingHorizontal);
    }


    private void MovementRelativeToCamera()// (Old) Moving Control
    {
        float playerVerticalInput = Input.GetAxis("Vertical");
        float playerHorizontalInput = Input.GetAxis("Horizontal");

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 forwardRelativeVerticalInput = rotatingHorizontal * forward;
        Vector3 rightRelativeHorizontalInput = rotatingVertical * right;

        Vector3 cameraRelativeMovement = (forwardRelativeVerticalInput + rightRelativeHorizontalInput) * movementSpeed;
        //this.transform.Translate(cameraRelativeMovement, Space.World);
        playerRb.AddForce(cameraRelativeMovement, ForceMode.Force);

        Vector3 rotateRelativeMovement = cameraRelativeMovement.normalized;
        surfaceAligning.movement = rotateRelativeMovement;
        if(rotateRelativeMovement != Vector3.zero)
        {
            //transform.forward =
            //transform.forward = rotateRelativeMovement;
            //movementToCameraRot = 

            Quaternion toRotation = Quaternion.LookRotation(rotateRelativeMovement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            //playerRb.AddRelativeForce(Vector3.forward * movementSpeed, ForceMode.Force);

            animationController.RunningAnimation();
        }
        else
        {
            animationController.IdleAnimation();
        }
    }
    private void SurfaceAlignment()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit info = new RaycastHit();
        //Quaternion RotationRef = Quaternion.Euler(0, 0, 0);
        //Debug.Log(info);
        if (Physics.Raycast(ray, out info, WhatIsGround))
        {
            //transform.rotation =
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, info.normal), animCurve.Evaluate(gTime));
            //surfaceAlignmentRot =
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        if(other.gameObject.CompareTag("SpeedBoost"))
        {
            Debug.Log("Speed Boost");
            movementSpeed = 225;
            speedBuffTime = 0f;
            speedBuff = true;
        }
    }
}