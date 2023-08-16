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
        if(!_stats.isMoving)
            targetSpeed = 0;
        float speedChangeRate = _stats.isBoosting? _stats.boostingAccelerationRate : _stats.normalAccelerationRate;
        float speedOffset = 0.1f;

        if(_speed < targetSpeed - speedOffset)
        {
            _speed += speedChangeRate * Time.deltaTime;

            // round speed to 2 decimal places
            _speed = Mathf.Round(_speed * 100f) / 100f;
        }
        else if(_speed > targetSpeed + speedOffset)
        {
            _speed -= speedChangeRate * Time.deltaTime;

            _speed = Mathf.Round(_speed * 100f) / 100f;
        }
        
        transform.Translate(Vector3.forward * Mathf.Clamp(_speed, 0, 100f) * Time.deltaTime);
    }
    

    public IEnumerator Recenter() ///broken
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

}
