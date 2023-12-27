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
    [SerializeField] private NPlayerStateController _playerStates;
    public NPlayerScriptableObject _stats;
    [SerializeField] private NPlayerOpenControl openControl;
    [SerializeField] private CinemachineFreeLook _mainCamera;
    public float LookSensitivity;

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
        LookInput(value.Get<Vector2>() * LookSensitivity);
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
    public void OnPause(InputValue value)
    {
        PauseGame();
    }
    public void OnContinue(InputValue value)
    {

    }
    public void OnTightTurn(InputValue value)
    {
        TightTurnInput(value.Get<float>());
    }
    public void OnReOrientatePlayer(InputValue value)
    {
        ReOrientateInput();
    }
    #endif

    void MoveInput(float movementMultiplier)
    {
        if(!_stats.isLevelPlayer)
        {
            if(movementMultiplier > 0) _stats.isMoving = true;
            else _stats.isMoving = false;
            bool newMoveState = _stats.isMoving;
            _stats.MovementMultiplier = movementMultiplier;
            if(!newMoveState) StartCoroutine(openControl.ReAdjustPlayer());
        }
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
        StartCoroutine(openControl.RecenterCamera());
    }
    void ReOrientateInput()
    {
        StartCoroutine(openControl.ReAdjustPlayer());
    }
    void TightTurnInput(float multiplier)
    {
        _stats.TurningMultiplier = multiplier + 1;
    }

    void PauseGame()
    {
        _playerStates.GamePaused = !_playerStates.GamePaused;
    }
}
