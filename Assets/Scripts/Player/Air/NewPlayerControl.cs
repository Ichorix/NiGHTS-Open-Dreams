using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class NewPlayerControl : MonoBehaviour
{
    PlayerInputActions controls;
    public StateController stateController;
    public GameObject trailInstantiator;
    public Rigidbody playerRb;
    public BoostBar boostBar;
    public TextMeshProUGUI chipText;
    
    public Vector2 rotate;
    [SerializeField] private Quaternion PlayerRot;

    [SerializeField] private bool isMoving, isBoosting, BoostAttempt, power;
    private int maxBoost;
    public float speed;
    private float rotationSpeed, BoostTime, powerTime;
    [SerializeField] private float normStartSpeed, normMaxSpeed, normSpeedaBoosting, boostAttemptSpeed, boostingStartSpeed, boostingMaxSpeed;
    [SerializeField] private float normAcceleration, boostingAcceleration;
    [SerializeField] private float accelerationMult;
    [SerializeField] private float decelMult;

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

        
    }
    void OnDisable()
    {
        controls.Player.Disable();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            power = true;
            powerTime = -999f;
        }


        //Boost Amount
        boostBar.SetBoost((int)BoostGauge);

        if(BoostGauge <= 0 && isMoving == true)
        {
            //speed = normStartSpeed;
            isBoosting = false;
        }

        //BoostAttempts
        if(BoostAttempt == true)
        {
            speed = boostAttemptSpeed;
            BoostTime += Time.deltaTime;
        }
        if(BoostTime >= 0.33f && isMoving == true)
        {
            BoostAttempt = false;
            speed = normStartSpeed;
            BoostTime = 0;
        }
        if(BoostTime >= 0.33f && isMoving == false)
        {
            BoostAttempt = false;
            speed = 0;
            BoostTime = 0;
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
        if(power == true)
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
            isMoving = true;
            speed = normStartSpeed;
            accelerationMult = normAcceleration;
            trailInstantiator.SetActive(true);
            //Gamepad.current.SetMotorSpeeds(.25f, .25f);
        }
        if(context.canceled)
        {
            isMoving = false;
            speed = 0f;
            trailInstantiator.SetActive(false);
            //Gamepad.current.SetMotorSpeeds(0f,0f);

            //Reorientation
            transform.rotation = PlayerRot;            
        }
        if(isBoosting == true && context.started)
        {
            isMoving = true;
            accelerationMult = boostingAcceleration;
            speed = boostingStartSpeed;
            trailInstantiator.SetActive(true);
            //Gamepad.current.SetMotorSpeeds(.5f, .6f)
        }
    }
    
    public void Boosting(InputAction.CallbackContext context)
    {
        if(0 <= BoostGauge && isMoving == true && context.started)
        {
            isBoosting = true;
            if(speed < boostingStartSpeed)
            {
                speed = boostingStartSpeed;
            }
            accelerationMult = boostingAcceleration;
            BoostGauge -= 1;
            //Gamepad.current.SetMotorSpeeds(.5f, .6f);
            Sounds.PlayOneShot(BoostStart, 1.0f);
            boostingSource.PlayScheduled(AudioSettings.dspTime + BoostStart.length);
        }
        if(context.canceled && isMoving == true)
        {
            isBoosting = false;
            if(speed < normStartSpeed) speed = normStartSpeed;
            else
            {
                if(speed > normSpeedaBoosting) speed = normSpeedaBoosting;
                else speed = speed;
            }
            accelerationMult = normAcceleration;
            //Gamepad.current.SetMotorSpeeds(.25f, .25f);
            boostingSource.Stop();
            Sounds.PlayOneShot(BoostEnd, 1.0f);
        }
        if(0 >= BoostGauge && context.started)
        {
            isBoosting = false;
            BoostAttempt = true;
            speed = normStartSpeed;
            //Gamepad.current.SetMotorSpeeds(.25f, .25f);
        }
        if(isMoving == false)
        {
            isBoosting = true;
            speed = 0f;
            //Gamepad.current.SetMotorSpeeds(0f,0f);
        }
        if(context.canceled)
        {
            isBoosting = false;
        }
    }

    public int chipAmount;
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("lb_groundTarget"))
        {
            Debug.Log("Ground");
            stateController.Activate2();
        }
        if(other.gameObject.CompareTag("BlueChip"))
        {
            CollectBlueChip(other);
        }
        if(other.gameObject.CompareTag("YellowRing"))
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
        if(other.gameObject.CompareTag("GreenRing"))
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
        if(other.gameObject.CompareTag("HalfRing"))
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
        if(other.gameObject.CompareTag("PowerRing"))
        {
            Sounds.PlayOneShot(PowerRingSFX, 1.0f);
            BoostGauge = 100;
            power = true;
        }
        if(other.gameObject.CompareTag("SpikeRing"))
        {
            Sounds.PlayOneShot(SpikeRingSFX, 1.0f);
            BoostGauge -= 5;
        }
    }

    public void CollectBlueChip(Collider other)
    {
        Sounds.PlayOneShot(BlueChipSFX, 1.0f);
        other.gameObject.SetActive(false);
        chipAmount += 1;
    }
}

