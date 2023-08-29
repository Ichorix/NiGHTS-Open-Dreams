using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif


public class NPlayerInput : MonoBehaviour
{
    public NPlayerScriptableObject _stats;
    public NPlayerOpenControl _openPlayer;
    
    #if ENABLE_INPUT_SYSTEM
    public void OnMoving(InputValue value)
    {
        MoveInput(value.isPressed);
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
    void MoveInput(bool newMoveState)
    {
        _stats.isMoving = newMoveState;
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
