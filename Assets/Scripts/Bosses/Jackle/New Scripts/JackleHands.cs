using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackleHands : MonoBehaviour
{
    public LineRenderer lineRend;
    public AnimationCurve curve;
    public Animator animator;
    public GameObject body;
    public GameObject target;
    public Vector3 moveToPos;
    public Quaternion normalRotation;
    //public Rigidbody rigidbody;

    public float speed;
    public float aimingTime;
    public float chasingSpeed = 5;
    public float bounceSpeed = 7;
    public float defaultSpeed = 10;
    public float sawingSpeed = 25;
    public float punchSpeed = 150;
    public float returningTime;
    public float maxAimingTime;
    public float stunnedTime;
    public float recoveryTime;
    public float randx;
    public float randy;
    public Vector3 knockbackDir;
    public float knockbackForce = 1;
    public Vector3 oldForward;

    public bool active;
    public bool returning;
    public bool isGrabbing;
    public bool isSawing;
    public bool bounceSaw;
    public bool aimingSingle;
    public bool aimingDouble;
    public bool punch;
    public bool isTop;
    public bool stunned; /// CHECK AND FINISH STUNNED
    public bool isAvailable; /// SET AVAILABILITY SETTINGS
    
    public float aimingDifference;
    public Vector3 aimingLocation;
    
    void Start()
    {
        normalRotation = transform.rotation;
        moveToPos = transform.position;
        speed = defaultSpeed;
        active = true;
        Return();
    }
    void Update()
    {
        if(isGrabbing)
        {
            moveToPos = target.transform.position;
            transform.LookAt(target.transform);
        }
        if(isSawing)
        {
            //Debug.Log("Sawinggg");

            transform.RotateAround(body.transform.position, Vector3.forward, -speed);

            //transform.rotation += new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z + Time.deltaTime * speed, transform.rotation.w);
        }
        if(aimingSingle)
        {
            aimingLocation = target.transform.position;

            lineRend.SetPosition(0, transform.position);
            lineRend.SetPosition(1, aimingLocation);

            transform.LookAt(aimingLocation);
            aimingTime += Time.deltaTime;
        }
        if(aimingDouble)
        {
            if(isTop)
            {
                aimingLocation = new Vector3(target.transform.position.x, target.transform.position.y + aimingDifference, target.transform.position.z);
            }
            else
            {
                aimingLocation = new Vector3(target.transform.position.x, target.transform.position.y - aimingDifference, target.transform.position.z);
            }

            lineRend.SetPosition(0, transform.position);
            lineRend.SetPosition(1, aimingLocation);

            transform.LookAt(aimingLocation);
            aimingTime += Time.deltaTime;
        }
        if(aimingTime >= maxAimingTime)
        {
            ActivatePunch();
        }
        if(punch)
        {
            aimingTime = 0;
            transform.Translate(Vector3.forward * punchSpeed * Time.deltaTime);
        }
        if(bounceSaw)
        {
            animator.SetBool("BounceSaw", true);
            transform.Translate(direction * speed * Time.deltaTime);

            movingTo = new Vector3(transform.position.x + -direction.x * 20, transform.position.y + direction.y * 20, transform.position.z + direction.z * 20);
            lineRend.SetPosition(0, transform.position);
            lineRend.SetPosition(1, movingTo);
        }
        
        if(returning)
        {
            returningTime += Time.deltaTime * 0.66f;
            transform.position = Vector3.Lerp(transform.position, moveToPos, curve.Evaluate(returningTime));
        }

        if(active)
        {
            if(!isGrabbing)
            {
                moveToPos = body.transform.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, moveToPos, speed * Time.deltaTime);
        }
        if(stunned && stunnedTime < recoveryTime)/// Get a random location within like 100x100 of the hand then turn on active and make the moveToPos that place.
        {
            active = false;
            stunnedTime += Time.deltaTime;
            
            returningTime += Time.deltaTime * 0.66f;
            transform.position = Vector3.Lerp(transform.position, moveToPos, curve.Evaluate(returningTime));
        }
        if(stunned && stunnedTime >= recoveryTime)
        {
            Return();
        }


        //////////Debug Inputs//////////
        if(Input.GetKeyDown(KeyCode.R))//Return
        {
            Debug.Log("Return");
            Return();
        }
        if(Input.GetKeyDown(KeyCode.C))//Chase
        {
            Debug.Log("Chase");
            Grab();
        }
        if(Input.GetKeyDown(KeyCode.X))//Saw
        {
            Debug.Log("Saw");
            HandSaw();
        }
        if(Input.GetKeyDown(KeyCode.L))//Punch
        {
            Debug.Log("Punch");
            Punch(false);
        }
        if(Input.GetKeyDown(KeyCode.J))//Double Punch
        {
            Debug.Log("DoublePunch");
            Punch(true);
        }
        if(Input.GetKeyDown(KeyCode.T))//Bounce Saw
        {
            Debug.Log("Bounce Saw");
            BounceSaw(isTop);
        }
    }

    public void Return()
    {
        returningTime = 0;
        active = true;
        stunned = false;
        returning = true;
        isGrabbing = false;
        //animator.SetBool("isGrab", false);
        isSawing = false;
        //animator.SetBool("OuterSaw", false);
        punch = false;
        //animator.SetBool("isPunch", false);
        bounceSaw = false;
        animator.SetBool("BounceSaw", false);
        lineRend.enabled = false;
        animator.SetTrigger("TrIdle");
        transform.rotation = normalRotation;
        moveToPos = body.transform.position;
        isAvailable = true;
    }
    public void Grab()
    {
        isGrabbing = true;
        isAvailable = false;
        returning = false;
        animator.SetTrigger("TrGrab");
        moveToPos = target.transform.position;
        speed = chasingSpeed;
    }
    public void Punch(bool isDouble)
    {
        lineRend.enabled = true;
        animator.SetTrigger("TrPunch");
        if(!isDouble)
        {
            Debug.Log(this.name + "Normal Punch");
            aimingTime = 0;
            aimingSingle = true;
            aimingDouble = false;
        }
        if(isDouble)
        {
            aimingTime = 0;
            aimingSingle = false;
            aimingDouble = true;
            Debug.Log(this.name + "Double Punch");
        }
    }
    public void ActivatePunch()
    {
        returning = false;
        active = false;
        isAvailable = false;
        lineRend.enabled = false;
        aimingSingle = false;
        aimingDouble = false;
        punch = true;
    }
    public void HandSaw()
    {
        isAvailable = true;
        normalRotation = transform.rotation;
        isSawing = true;
        animator.SetTrigger("OuterSaw");
        speed = sawingSpeed;
    }
    
    public Vector3 direction;
    public Vector3 movingTo;
    public float leftRight;
    public void BounceSaw(bool LRdirection/*false is left, true is right*/)
    {
        isAvailable = false;
        returning = false;
        leftRight = 1;
        if(LRdirection) leftRight = -1;
        lineRend.enabled = true;
        bounceSaw = true;
        animator.SetTrigger("OuterSaw");
        active = false;
        speed = bounceSpeed;
        direction = new Vector3(0.5f * leftRight, -1, 0);
    }
    
    public void Stunned()
    {
        Debug.Log(this.name + "Was Stunned");
        active = false;
        isAvailable = false;
        stunned = true;
        stunnedTime = 0;
        returningTime = 0;

        randx = Random.Range(-5, 5);
        randy = Random.Range(-5, 5);

        moveToPos = new Vector3(transform.position.x + randx, transform.position.y + randy, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(this.name + "Collided with something");
        if(bounceSaw)
        {
            Debug.Log("Bounce");
            direction = new Vector3(direction.x, -direction.y, 0);
            //make tags for ground and walls
        }
        
    }
}
