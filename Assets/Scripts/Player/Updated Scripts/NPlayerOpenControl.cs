using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class NPlayerOpenControl : MonoBehaviour
{
    
    public NPlayerScriptableObject _stats;
    public NPlayerAnimations _animations;
    public CinemachineFreeLook cameraSettings;
    [Space]
    [SerializeField] private bool canBoost;
    [SerializeField] private bool boostAttempt;
    [SerializeField] private bool boostAttemptCooldown;
    [SerializeField] private float _speed;
    public GameObject CinemachineCameraTarget;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    public float Sensitivity;
    public float CameraAngleOverride;
    private const float _threshold = 0.01f;
    private float timeOffset;
    public bool IsCurrentDeviceMouse;


    void Start()
    {
        _stats.boostGauge = _stats.maxBoost;
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }
    void Update()
    {
        RotatePlayer();
        if(!boostAttempt) MovePlayer();
        BoostStuff();
    }
    void LateUpdate()
    {
        CameraRotation();
    }
    private void RotatePlayer()
    {
        Vector2 rotation = new Vector2(-_stats.MoveDirection.y, _stats.MoveDirection.x); //I don't remember why they have to be reordered, but whatever
        transform.Rotate(_stats.turningSpeed * rotation * Time.deltaTime);
    }
    private void MovePlayer()
    {
        canBoost = false;
        if(_stats.isBoosting) canBoost = true;
        if(_stats.boostGauge <= 0)
        {
            canBoost = false;
            if(_stats.isMoving && _stats.runBoostAttempt && !boostAttemptCooldown)
            {
                _stats.runBoostAttempt = false;
                boostAttempt = true;
                RunBoostAttempt();
            }
            else _stats.runBoostAttempt = false;
        }

        float targetSpeed = canBoost ? _stats.boostingSpeed : _stats.normalSpeed;
        
        //Checks if you are going faster than the target speed (true when target speed is 0 or when target speed is normal speed after boosting)
        //And if your speed is greater than the normal speed after boosting
        if(_speed >= targetSpeed && _speed > _stats.speedABoosting) // Will return true if you are decelerating after boosting.
            targetSpeed = _stats.speedABoosting;                    
        // If the speed change rate is too high or if performance is bad, it will sometimes jump past speedABoosting resulting in decelerating to normal speed
        // This can be fixed by increasing the speedOffset.

        if(!_stats.isMoving)
            targetSpeed = 0;
        float speedChangeRate = _stats.isBoosting? _stats.boostingAccelerationRate : _stats.normalAccelerationRate;
        float speedOffset = 0.5f; //Default 0.5f

        if(_speed < targetSpeed - speedOffset) //Accelerate
        {
            _speed += speedChangeRate * Time.deltaTime;

            // round speed to 2 decimal places
            _speed = Mathf.Round(_speed * 100f) / 100f;
        }
        else if(_speed > targetSpeed + speedOffset) //Decelerate
        {
            if(targetSpeed == 0) speedChangeRate = _stats.decelerationRate;

            _speed -= speedChangeRate * Time.deltaTime;

            _speed = Mathf.Round(_speed * 100f) / 100f;
        }
        if(_speed <= speedOffset) _speed = 0;
        
        transform.Translate(Vector3.forward * Mathf.Clamp(_speed, 0, 100f) * Time.deltaTime);
    }
    private void BoostStuff()
    {
        if(canBoost && _stats.isMoving)
            _stats.boostGauge -= _stats.boostDepletionRate * Time.deltaTime;
        _stats.boostGauge = Mathf.Clamp(_stats.boostGauge, 0, _stats.maxBoost);
    }
    private void CameraRotation()
    {
        if(_stats.LookDirection.sqrMagnitude >= _threshold)
        {
            
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _stats.LookDirection.x * deltaTimeMultiplier * Sensitivity;
            _cinemachineTargetPitch += _stats.LookDirection.y * deltaTimeMultiplier * Sensitivity; 
        }

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    public IEnumerator RecenterCamera()
    {
        Debug.Log("Recenteringgg");
        cameraSettings.m_RecenterToTargetHeading = new AxisState.Recentering(true, 0, 0.25f);
        yield return new WaitForSeconds(1);
        cameraSettings.m_RecenterToTargetHeading = new AxisState.Recentering(false, 0, 0.25f);
    }
    public void CamSetWorld()
    {
        if(cameraSettings != null)
        {
            cameraSettings.m_BindingMode = CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;
            _stats.cameraPlayerBound = false;
        }
    }
    public void CamSetPlayer()
    {
        if(cameraSettings != null)
        {
            cameraSettings.m_BindingMode = CinemachineTransposer.BindingMode.LockToTarget;
            _stats.cameraPlayerBound = true;
        }
    }

    public void StoppedMoving()
    {
        StartCoroutine(ReAdjustPlayer());
    }
    IEnumerator ReAdjustPlayer()
    {
        float t = 0;
        Quaternion fromRotation = transform.rotation;
        Quaternion toRotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);

        while(!_stats.isMoving && t < 1)
        {
            t += Time.deltaTime * _stats.recenterSpeed;
            transform.rotation = Quaternion.Slerp(fromRotation, toRotation, t);
            yield return null;
        }
    }

    public void RunBoostAttempt()
    {
        StartCoroutine(BoostAttempt());
    }
    IEnumerator BoostAttempt()
    {
        float t = 0;
        while(boostAttempt)
        {
            t += Time.deltaTime;
            transform.Translate(Vector3.forward * Mathf.Clamp(_stats.boostAttemptSpeed, 0, 100f) * Time.deltaTime);
            _animations.BoostAnimationOverride(true);
            if(t >= _stats.boostAttemptTime)
            {
                boostAttempt = false;
                boostAttemptCooldown = true;
            }
            yield return null;
        }
        t = 0;
        while(boostAttemptCooldown)
        {
            t += Time.deltaTime;
            _animations.BoostAnimationOverride(false);
            if(t >= _stats.boostAttemptCooldown)
                boostAttemptCooldown = false;
            yield return null;
        }
        _stats.runBoostAttempt = false;
    }
}
