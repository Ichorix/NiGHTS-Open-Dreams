using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class NPlayerOpenControl : MonoBehaviour
{
    [Header("Save Data")]
    [SerializeField] private int openChips;
    public int OpenChips
    {
        get
        {
            if(PlayerPrefs.HasKey("openChips"))
                openChips = PlayerPrefs.GetInt("openChips");
            return openChips;
        }
        set
        {
            openChips = value;
            PlayerPrefs.SetInt("openChips", openChips);
        }
    }
    [Space]
    [Header("References")]
    [SerializeField] private NPlayerScriptableObject _stats;
    public NPlayerAnimations _animations;
    [SerializeField] private ParaloopEmitter trailInstantiator;
    private Rigidbody rigidbody;
    [Space]
    [Header("Links Data")]
    [SerializeField] private LinkControl linkControl;
    public int link;
    [SerializeField] private bool linkActive;
    [SerializeField] private float linkTimeLeft;
    public float LinkTimeLeft
    {
        get{ return linkTimeLeft; }
        set
        {
            if(value <= 0) LinkEmpty();
            linkTimeLeft = value;
        }
    }
    [Space]
    [Header("Movement Information")]
    [SerializeField] private bool canBoost;
    [SerializeField] private bool boostAttempt;
    [SerializeField] private bool boostAttemptCooldown;
    public float _speed;
    private Vector3 mostRecentGroundNormal;
    [Space]
    [Header("Camera Information")]
    [Range(0, 1)]
    public float angleToSwitchCamera;
    [Range(1, 2)]
    public float tightTurnValueToSwitchCamera;
    public AnimationCurve fieldOfViewBySpeed;
    public CinemachineFreeLook cameraLock;
    public CinemachineFreeLook cameraFollow;


    void OnDisable()
    {
        trailInstantiator.enabled = false;
    }
    void Start()
    {
        OpenChips = OpenChips; // Load the saved value
        rigidbody = GetComponent<Rigidbody>();
        _stats.BoostGauge = _stats.maxBoost;
    }

    void Update()
    {
        RotatePlayer();
        if(!boostAttempt) MovePlayer(CalculateMomentumBonus());
        BoostStuff();
        ActivateParaloop();
        if(linkActive) LinkTimeLeft -= Time.deltaTime;
    }

    void LateUpdate()
    {
        CameraFunctions();
    }


    private void RotatePlayer()
    {
        // I don't remember why they have to be reordered, but whatever
        Vector2 rotation = new Vector2(-_stats.MoveDirection.y, _stats.MoveDirection.x);
        transform.Rotate(_stats.turningSpeed * _stats.TurningMultiplier * rotation * Time.deltaTime);
    }

    private float CalculateMomentumBonus()
    {
        // Results in a value between -1 and 1 with precision to 2 decimal places
        // yForward is -1 when looking straight up, and 1 when looking straight down
        float yForward = Mathf.Round(-transform.forward.y * 100f) * 0.01f; 
        
        if(yForward >= 0)
            yForward *= _stats.downwardsMomentumMultiplier;
        else if(yForward < 0)
            yForward *= _stats.upwardsMomentumMultiplier;
        
        return yForward;
    }

    private void MovePlayer(float momentumBonus)
    {
        canBoost = false;
        if(_stats.isBoosting) canBoost = true;

        if(_stats.BoostGauge <= 0)
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

        // The target speed is the speed that _speed tries to get to through Acceleration and Deceleration
        // Checks if you should be boosting and sets it to the appropriate speed + the inputted bonus from your up/down angle
        // If you are not boosting however, It will lerp between 0 and the normal moving speed based on _stats.MovementMultiplier
        // _stats.MovementMultiplier is the value recieved by the input to move. On keyboard it will either be 1, pressed, or 0, not pressed
        // However on Gamepad it ranges from 0, not pressed, 0.5, half pressed, and 1, fully pressed.
        float targetSpeed = canBoost ? _stats.boostingSpeed + momentumBonus : Mathf.Lerp(0, _stats.normalSpeed, _stats.MovementMultiplier) + momentumBonus;
        
        // Checks if you are going faster than the target speed (true when target speed is 0 or when target speed is normal speed after boosting)
        // And if your speed is greater than the normal speed after boosting
        // Will return true if you are decelerating after boosting.
        if(_speed >= targetSpeed && _speed > _stats.speedABoosting)
            targetSpeed = _stats.speedABoosting;                    
        // If the speed change rate is too high or if performance is bad, it will sometimes jump past speedABoosting resulting in decelerating to normal speed
        // This can be fixed by increasing the speedOffset.
        // Increasing speedOffset too much can cause bugs with acceleration on higher framerates, increase with moderation.
        // Essentially you want the highest value possible to catch speedABoosting, that is still less than the speedChangeRate for proper acceleration
        // To Test issues, compare speedChangeRate * Time.deltaTime and speedOffset.
        // If speedChangeRate goes below the speed offset, then acceleration will break

        if(!_stats.isMoving)
            targetSpeed = 0;
        
        float speedChangeRate = _stats.isBoosting? _stats.boostingAccelerationRate : _stats.normalAccelerationRate;
        float speedOffset = 10f * Time.deltaTime; // Instead of having a static number, multiplying by Time.deltaTime should keep the values in balance

        

        if(_speed < targetSpeed - speedOffset) //Accelerate
        {
            _speed += speedChangeRate * Time.deltaTime;

            // round speed to 2 decimal places
            _speed = Mathf.Round(_speed * 100f) * 0.01f;
        }
        else if(_speed > targetSpeed + speedOffset) //Decelerate
        {
            if(targetSpeed == 0) speedChangeRate = _stats.decelerationRate;

            _speed -= speedChangeRate * Time.deltaTime;

            _speed = Mathf.Round(_speed * 100f) * 0.01f;
        }
        if(_speed <= speedOffset) _speed = 0;

        ///// If speed acts funky, activate these to determine it
        //Debug.Log("DeltaTime = " + Time.deltaTime);
        //Debug.Log("Target Speed = " + targetSpeed + ", Current Speed = " + _speed);
        //Debug.Log("Speed Change Rate = " + speedChangeRate * Time.deltaTime + "Speed Offset = " + speedOffset);
        //Debug.Log("Movement Multiplier = " + _stats.MovementMultiplier + ", Momentum Bonus = " + momentumBonus);
        transform.Translate(Vector3.forward * Mathf.Clamp(_speed, 0, 100f) * Time.deltaTime);
    }
    private void ActivateParaloop()
    {
        trailInstantiator.enabled = _stats.isMoving;
    }
    private void BoostStuff()
    {
        if(_stats.PowerBuff)
        {
            _stats.BoostGauge = _stats.maxBoost;
            _stats.PowerBuffTimeLeft -= Time.deltaTime;
            return; // We dont need to do any more calculations if you have Power active
        }

        if(canBoost && _stats.isMoving)
            _stats.BoostGauge -= _stats.boostDepletionRate * Time.deltaTime;
    }
    
    private void CameraFunctions()
    {
        // FOV
        float fov = fieldOfViewBySpeed.Evaluate(_speed);
        cameraLock.m_Lens = new LensSettings(fov, 10f, 0.1f, 5000f, 0);
        cameraFollow.m_Lens = new LensSettings(fov, 10f, 0.1f, 5000f, 0);
        
        // Camera Angle
        cameraLock.m_YAxis.Value = 0.6f;
        cameraFollow.m_YAxis.Value = 0.65f;

        // Camera Locking
        if(transform.up.y < angleToSwitchCamera)
            cameraLock.gameObject.SetActive(true);
        else
            cameraLock.gameObject.SetActive(false);

        if(_stats.TurningMultiplier > tightTurnValueToSwitchCamera)
            cameraLock.gameObject.SetActive(false);
        
        if(canBoost)
            cameraLock.gameObject.SetActive(true);
        
    }
    public IEnumerator RecenterCamera()
    {
        cameraLock.m_RecenterToTargetHeading = new AxisState.Recentering(true, 0, 0.25f);
        cameraFollow.m_RecenterToTargetHeading = new AxisState.Recentering(true, 0, 0.25f);
        yield return new WaitForSeconds(1);
        cameraLock.m_RecenterToTargetHeading = new AxisState.Recentering(true, 0, 1f);
        cameraFollow.m_RecenterToTargetHeading = new AxisState.Recentering(true, 0, 1f);
    }

    public IEnumerator ReAdjustPlayer()
    {
        float t = 0;

        while(t < 1)
        {
            Quaternion fromRotation = transform.rotation;
            Quaternion toRotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);

            t += Time.deltaTime * _stats.recenterSpeed;
            transform.rotation = Quaternion.Slerp(fromRotation, toRotation, t);
            yield return null;
        }
    }


    // Called when you touch the ground
    public void ReAdjustToNormals(Vector3 groundNormal)
    {
        mostRecentGroundNormal = groundNormal;
        Vector3 normalForward = Vector3.Cross(mostRecentGroundNormal, -transform.right);

        // Calculates how far the player should rotate towards the intended forward by the speed inputted and how close the player is to that direction
        float step = _stats.groundAdjustSpeed * Vector3.Distance(transform.forward, normalForward) * Time.deltaTime;
        // Move towards the ground normal retaining the general forward direction by using the Cross Product of the current right vector
        transform.forward = Vector3.MoveTowards(transform.forward, normalForward, step);
    }

    // See NPlayerCollisionController for the 2 usage examples
    public void BumpUpFromGround(float bumpForce, float translateDistance = 0)
    {
        rigidbody.AddForce(mostRecentGroundNormal * bumpForce, ForceMode.Impulse);
        transform.Translate(mostRecentGroundNormal * translateDistance);
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
            _stats.BoostAttempt = true;
            transform.Translate(Vector3.forward * Mathf.Clamp(_stats.boostAttemptSpeed + CalculateMomentumBonus(), 0, 100f) * Time.deltaTime);
            _animations.BoostAnimationOverride(true);
            if(t >= _stats.boostAttemptTime)
            {
                boostAttempt = false;
                boostAttemptCooldown = true;
            }
            yield return null;
        }

        t = 0;
        _stats.BoostAttempt = false;
        while(boostAttemptCooldown)
        {
            t += Time.deltaTime;
            _animations.BoostAnimationOverride(false);
            if(t >= _stats.boostAttemptCooldown)
                boostAttemptCooldown = false;
            yield return null;
        }
    }

    public void LinkIncrease()
    {
        LinkTimeLeft = 1;
        link += 1;
        linkActive = true;
        if(linkControl != null)
            linkControl.RunLinkIncrease(link);
    }
    public void LinkEmpty()
    {
        link = 0;
        linkActive = false;
        if(linkControl != null)
            linkControl.RunLinkIncrease(link);
    }
}
