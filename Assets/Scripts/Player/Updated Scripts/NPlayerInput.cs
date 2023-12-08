using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif


public class NPlayerInput : MonoBehaviour
{
    private PlayerInputActions playerControls;
    public NPlayerScriptableObject _stats;
    public NPlayerOpenControl _openPlayer;

    void Awake()
    {
        playerControls = new PlayerInputActions();
    }
    
    #if ENABLE_INPUT_SYSTEM
    public void OnMoving(InputValue value)
    {
        Debug.Log("On Move");
        MoveInput(value.isPressed, value.Get<float>());
        Debug.Log(value.Get<float>());

    }
    public void OnLook(InputValue value)
    {
        LookInput(value.Get<Vector2>());
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

    void MoveInput(bool newMoveState, float movementMultiplier)
    {
        //_stats.isMoving = newMoveState; Runs normally at 0.5 for movementMultiplier. Now using an If to be more precise
        if(movementMultiplier > 0.05f) _stats.isMoving = true;
        else _stats.isMoving = false;
        
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
