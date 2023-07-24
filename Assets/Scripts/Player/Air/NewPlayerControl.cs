using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class NewPlayerControl : MonoBehaviour
{
    PlayerInputActions controls;
    public StateController stateController;
    public GameObject trailInstantiator;
    public InstantiatePointItem pointItemScript;
    public AnimationControl anims;
    public CinemachineFreeLook cameraSettings;
    [SerializeField] private bool cameraPlayerBound;
    public Rigidbody playerRb;
    public BoostBar boostBar;
    public TextMeshProUGUI chipText;
    public Material blueChipMat;
    public bool allowVibration;
    
    public Vector2 rotate;
    [SerializeField] private Quaternion PlayerRot;

    public bool isMoving, isBoosting, BoostAttempt, power;
    private int maxBoost;
    public float speed;
    private float rotationSpeed, BoostTime, powerTime;
    [SerializeField] private float normStartSpeed, normMaxSpeed, normSpeedaBoosting, boostAttemptSpeed, boostingStartSpeed, boostingMaxSpeed;
    [SerializeField] private float normAcceleration, boostingAcceleration;
    [SerializeField] private float accelerationMult;
    [SerializeField] private float decelMult;
    public AnimationCurve curve;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float decelTime, decelLerpMult;
    [SerializeField] private bool decelLerp;
    public AnimationCurve vibrationCurve;
    [SerializeField] private Vector2 controllerVibration;
    [SerializeField] private Vector2 boostingSpeedVibration;


    public bool isDrifting;
    public float driftingPower;
    

    public float BoostGauge;

    public AudioSource Sounds;
    public AudioSource boostingSource;
    [SerializeField] private AudioClip BoostStart, BoostIng, BoostEnd;
    [SerializeField] public AudioClip BlueChipSFX, YellowRingSFX, GreenRingSFX, HalfRingSFX, PowerRingSFX, SpikeRingSFX;
    
    void Awake()
    {
        controls = new PlayerInputActions();
        controls.Player.Enable();
        controls.Player.Rotating.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Player.Rotating.canceled += ctx => rotate = Vector2.zero;
    }
    void Start()
    {
        chipAmount = 0;
        BoostGauge = 100;

        maxBoost = 100;
        rotationSpeed = 300f;
        isMoving = false;
        isBoosting = false;
        BoostAttempt = false;
        BoostTime = 0;
        power = false;

        boostBar.SetMaxBoost(maxBoost);

        normStartSpeed = 5;
        normMaxSpeed = 25;
        normSpeedaBoosting = 35;
        boostAttemptSpeed = 35;
        boostingStartSpeed = 10;
        boostingMaxSpeed = 75;
        normAcceleration = 10;
        boostingAcceleration = 20;
        decelMult = 5;
    }
    void OnEnable()
    {
        controls.Player.Enable();
        playerRb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        maxBoost = 100;
        rotationSpeed = 300f;
        isMoving = false;
        isBoosting = false;
        BoostAttempt = false;
        BoostTime = 0;
        power = false;
        trailInstantiator.SetActive(false);
        blueChipMat.SetFloat("_EmissionOn", 0f);
        
    }
    void OnDisable()
    {
        controls.Player.Disable();
    }
    void Update()
    {
        if(decelLerp)
        {
            /* //Using Mathf.Lerp: doesnt allow for very good reacceleration
            speed = Mathf.Lerp(0, currentSpeed, curve.Evaluate(decelTime));
            decelTime += Time.deltaTime * decelLerpMult;
            if(decelTime >= 1)
            {
                decelLerp = false;
                isMoving = false;
            }
            */
            //Using simple deceleration
            speed -= Time.deltaTime * decelLerpMult;
            if(speed <= normStartSpeed)
            {
                decelLerp = false;
                isMoving = false;
            }
        }


        //Boost Amount
        boostBar.SetBoost((int)BoostGauge);

        if(BoostGauge <= 0 && isMoving)
        {
            //speed = normStartSpeed;
            isBoosting = false;

            if(speed < normStartSpeed) speed = normStartSpeed;
            else if(speed > normSpeedaBoosting) speed = normSpeedaBoosting;
            //else speed = speed;
                
            accelerationMult = normAcceleration;

            if(boostingSource.isPlaying)
            {
                boostingSource.Stop();
                Sounds.PlayOneShot(BoostEnd, 1.0f);
            }
        }

        //BoostAttempts
        if(BoostAttempt)
        {
            speed = boostAttemptSpeed;
            BoostTime += Time.deltaTime;
        }
        if(BoostTime >= 0.33f)
        {
            BoostAttempt = false;
            BoostTime = 0;
            if(isMoving) speed = normMaxSpeed;
            else speed = 0;
        }

        //Reorienting
        PlayerRot = transform.rotation;
        PlayerRot.x = 0;
        PlayerRot.z = 0;

        //Turning
        Vector2 r = new Vector2(-rotate.y, rotate.x);
        transform.Rotate(rotationSpeed * r * Time.deltaTime);
        //Debug.Log(r);
        //playerRb.AddRelativeTorque(Vector3.forward * rotationSpeed * r * Time.deltaTime * 250);

        //Moving
        if(isMoving)
        {
            //Debug.Log(transform.rotation.y);
            if(speed > 0)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                //playerRb.AddForce(Vector3.forward * speed * Time.deltaTime, ForceMode.Acceleration); 
                if(isBoosting)
                {
                    BoostGauge -= Time.deltaTime * 10;
                }
                else if(speed > normMaxSpeed)
                {
                    speed -= Time.deltaTime * decelMult;
                }
            }

            if(isMoving && speed < normMaxSpeed)
            {
                speed += Time.deltaTime * accelerationMult;
            }
            if(isBoosting && speed >= normMaxSpeed && speed < boostingMaxSpeed)
            {
                speed += Time.deltaTime * accelerationMult;
            }
            if(allowVibration)
            {
                controllerVibration = Vector2.Lerp(Vector2.zero, boostingSpeedVibration, vibrationCurve.Evaluate(speed));
                Gamepad.current.SetMotorSpeeds(controllerVibration.x, controllerVibration.y);
            }
            
        }

        //Drifting
        /*if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Drifting");
            isDrifting = true;
        }
        if(Input.GetKeyUp(KeyCode.E))
        {
            isDrifting = false;
        }
        if(isDrifting)
        {
            playerRb.AddTorque(Vector3.forward * r * driftingPower);
        }*/
        
        chipText.text = chipAmount.ToString();

        //power
        if(power)
        {
            BoostGauge = 100;
            powerTime += Time.deltaTime;
        }
        if(powerTime >= 10)
        {
            powerTime = 0;
            power = false;
        }
    }

    public void Moving(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            decelLerp = false;
            isMoving = true;
            //speed = normStartSpeed;
            accelerationMult = normAcceleration;
            trailInstantiator.SetActive(true);
            if(isBoosting)
            {
                accelerationMult = boostingAcceleration;
                speed = boostingStartSpeed;
            }
        }
        if(context.canceled)
        {
            currentSpeed = speed;
            decelLerp = true;
            decelTime = 0;
            trailInstantiator.SetActive(false);
            if(allowVibration) Gamepad.current.SetMotorSpeeds(0,0);
            //Reorientation
            transform.rotation = PlayerRot;            
        }
    }
    
    public void Boosting(InputAction.CallbackContext context)
    {
        if(0 <= BoostGauge && isMoving && context.started)
        {
            decelLerp = false;
            isBoosting = true;
            if(speed < boostingStartSpeed)
            {
                speed = boostingStartSpeed;
            }
            accelerationMult = boostingAcceleration;
            BoostGauge -= 1;
            Sounds.PlayOneShot(BoostStart, 1.0f);
            boostingSource.PlayScheduled(AudioSettings.dspTime + BoostStart.length);
        }
        if(context.canceled)
        {
            isBoosting = false;
            if(isMoving)
            {
                if(speed < normStartSpeed) speed = normStartSpeed;
                else if(speed > normSpeedaBoosting) speed = normSpeedaBoosting;
                //else speed = speed;
                
                accelerationMult = normAcceleration;

                if(boostingSource.isPlaying)
                {
                    boostingSource.Stop();
                    Sounds.PlayOneShot(BoostEnd, 1.0f);
                }
            }
        }
        if(0 >= BoostGauge && context.started)
        {
            isBoosting = false;
            BoostAttempt = true;
            anims.mAnimator.SetTrigger("TrBoostAttempt");
            //speed = normStartSpeed;
        }
        if(!isMoving)
        {
            isBoosting = true;
            currentSpeed = speed;
            decelLerp = true;
            decelTime = 0;
        }
    }


    public void RecenterCamera(InputAction.CallbackContext context)
    {
        Debug.Log("RecenterCamera " + context);
        StartCoroutine(Recenter());
    }
    public void ChangeCamera(InputAction.CallbackContext context)
    {
        //Debug.Log("ChangeCamera " + context);
        if(context.performed)
        {
            Debug.Log("Set Camera to");
            if(cameraPlayerBound)
            {
                Debug.Log("World");
                CamSetWorld();
            }
            else CamSetPlayer();
        }
    }

    public int chipAmount;
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("lb_groundTarget"))
        {
            Debug.Log("Ground");
            stateController.Activate2();
        }
        if(other.CompareTag("BlueChip"))
        {
            CollectBlueChip(other);
        }
        if(other.CompareTag("YellowRing"))
        {
            Sounds.PlayOneShot(YellowRingSFX, 1.0f);
            if(BoostGauge <= 90)
            {
                BoostGauge += 10;
            }
            if(BoostGauge >= 90)
            {
                BoostGauge = 100;
            }
        }
        if(other.CompareTag("GreenRing"))
        {
            Sounds.PlayOneShot(GreenRingSFX, 1.0f);
            if(BoostGauge <= 90)
            {
                BoostGauge += 10;
            }
            if(BoostGauge >= 90)
            {
                BoostGauge = 100;
            }
        }
        if(other.CompareTag("HalfRing"))
        {
            Sounds.PlayOneShot(HalfRingSFX, 1.0f);
            if(BoostGauge <= 90)
            {
                BoostGauge += 10;
            }
            if(BoostGauge >= 90)
            {
                BoostGauge = 100;
            }
        }
        if(other.CompareTag("PowerRing"))
        {
            Sounds.PlayOneShot(PowerRingSFX, 1.0f);
            BoostGauge = 100;
            power = true;
        }
        if(other.CompareTag("SpikeRing"))
        {
            Sounds.PlayOneShot(SpikeRingSFX, 1.0f);
            BoostGauge -= 5;
        }
    }

    public void CollectBlueChip(Collider other)
    {
        pointItemScript.InstantiatePointAndChip(true);
        Sounds.PlayOneShot(BlueChipSFX, 1.0f);
        other.gameObject.SetActive(false);
        chipAmount += 1;
    }

    IEnumerator Recenter()
    {
        Debug.Log("Recenteringgg");
        cameraSettings.m_RecenterToTargetHeading = new AxisState.Recentering(true, 0, 0.25f);
        yield return new WaitForSeconds(1);
        cameraSettings.m_RecenterToTargetHeading = new AxisState.Recentering(false, 0, 0.25f);
        //public AxisState.Recentering m_RecenterToTargetHeading = new AxisState.Recentering(false, 1, 2);
    }

    public void CamSetWorld()
    {
        if(cameraSettings != null)
        {
            cameraSettings.m_BindingMode = CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;
            cameraPlayerBound = false;
        }
    }
    public void CamSetPlayer()
    {
        if(cameraSettings != null)
        {
            cameraSettings.m_BindingMode = CinemachineTransposer.BindingMode.LockToTarget;
            cameraPlayerBound = true;
        }
    }
}

