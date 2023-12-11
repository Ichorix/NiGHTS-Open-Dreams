using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif


public class NPlayerInput : MonoBehaviour
{
    private PlayerInputActions playerControls;
    public NPlayerScriptableObject _stats;
    public NPlayerOpenControl _openPlayer;
    public CinemachineFreeLook _mainCamera;

    void Awake()
    {
        playerControls = new PlayerInputActions();
    }
    
    #if ENABLE_INPUT_SYSTEM
    public void OnMoving(InputValue value)
    {
        MoveInput(value.Get<float>());
    }
    public void OnLook(InputValue value)
    {
        LookInput(value.Get<Vector2>() * _stats.JoystickLookSensitivity);
    }
    public void OnBoosting(InputValue value)
    {
        BoostInput(value.isPressed);
        if(value.isPressed) _stats.runBoostAttempt = true;
    }
    public void OnRotating(InputValue value)
    {
        TurnInput(value.Get<Vector2>());
    }
    public void OnRecenterCamera(InputValue value)
    {
        RecenterCamera();
    }
    public void OnChangeCamera(InputValue value)
    {
        ChangeCamera(value.isPressed);
    }
    public void OnPause(InputValue value)
    {
        
    }
    public void OnContinue(InputValue value)
    {

    }
    #endif
    void Update()
    {
        LookInput(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * _stats.MouseLookSensitivity);
        //_mainCamera.m_YAxis = new AxisState(0.5f, 2, )
    }

    void MoveInput(float movementMultiplier)
    {
        //_stats.isMoving = newMoveState; Runs normally at 0.5 for movementMultiplier. Now using an If to be more precise
        if(movementMultiplier > 0) _stats.isMoving = true;
        else _stats.isMoving = false;
        bool newMoveState = _stats.isMoving;
        //Debug.Log(newMoveState + ", " + movementMultiplier);
        _stats.MovementMultiplier = movementMultiplier;
        if(!newMoveState) _openPlayer.StoppedMoving();
    }
    void BoostInput(bool newBoostState)
    {
        _stats.isBoosting = newBoostState;
    }
    void TurnInput(Vector2 dir)
    {
        _stats.MoveDirection = dir;
    }
    void LookInput(Vector2 dir)
    {
        //Debug.Log(dir);
        _stats.LookDirection = dir;


    }

    void RecenterCamera()
    {
        StartCoroutine(_openPlayer.RecenterCamera());
    }
    void ChangeCamera(bool isPerformed)
    {
        if(isPerformed)
        {
            Debug.Log("Set Camera to");
            if(_stats.cameraPlayerBound)
            {
                Debug.Log("World");
                _openPlayer.CamSetWorld();
            }
            else
            {
                Debug.Log("Player");
                _openPlayer.CamSetPlayer();
            }
        }
    }
}
