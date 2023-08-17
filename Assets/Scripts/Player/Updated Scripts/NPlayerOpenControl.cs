using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NPlayerOpenControl : MonoBehaviour
{
    public NPlayerScriptableObject _stats;
    //public NPlayerInput _input;
    public CinemachineFreeLook cameraSettings;
    [Space]
    [SerializeField] private float _speed;
    void Update()
    {
        RotatePlayer();
        MovePlayer();
    }
    private void RotatePlayer()
    {
        Vector2 rotation = new Vector2(-_stats.MoveDirection.y, _stats.MoveDirection.x); //I don't remember why they have to be reordered, but whatever
        transform.Rotate(_stats.turningSpeed * rotation * Time.deltaTime);
    }
    private void MovePlayer()
    {
        float targetSpeed = _stats.isBoosting ? _stats.boostingSpeed : _stats.normalSpeed;
        
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
    

    public IEnumerator Recenter()
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
}
