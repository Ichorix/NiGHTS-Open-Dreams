using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationControl : MonoBehaviour
{
    public NewPlayerControl script;
    public GameObject player;
    public Vector2 rotate;
    public Vector2 angle;
    public Animator mAnimator;
    PlayerInputActions controls;
    public ParticleSystem LeftHandSparkles, RightHandSparkles, BoostTrail;
    public TrailRenderer trail;
    public float speedWindThreshold;
    public ParticleSystem windEffects;
    public AudioSource windSFX;

    public bool isMoving, isBoosting, isLeft, isRight, TurnAnim;
    public float BoostGauge, BoostNum;


    void Awake()
    {
        script = player.GetComponent<NewPlayerControl>();
        BoostGauge = script.BoostGauge;
        rotate = script.rotate;
    }
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        
        //Particles
        LeftHandSparkles.enableEmission = false;
        RightHandSparkles.enableEmission = false;
        BoostTrail.enableEmission = false;
        trail.emitting = false;
        TurnAnim = false;
    }
    void Update()
    {
        BoostGauge = script.BoostGauge;
        angle = script.rotate;
        BoostNum = Random.Range(1,2);

        isMoving = script.isMoving;
        isBoosting = script.isBoosting;
        
        Vector2 a = new Vector2(angle.x, angle.y) * Time.deltaTime;

        //Particles
        if(isMoving)
        {
            mAnimator.SetBool("isMoving", true);
            LeftHandSparkles.enableEmission = true;
            RightHandSparkles.enableEmission = true;
        }
        if(!isMoving)
        {
            LeftHandSparkles.enableEmission = false;
            RightHandSparkles.enableEmission = false;
            mAnimator.SetBool("isMoving", false);
        }
        if(isBoosting)
        {
            BoostTrail.enableEmission = true;
            trail.emitting = true;
        }
        if(!isBoosting)
        {
            BoostTrail.enableEmission = false;
            trail.emitting = false;
            mAnimator.SetBool("isBoosting", false);
        }
        if(script.speed >= speedWindThreshold)
        {
            windEffects.enableEmission = true;
            if(!windSFX.isPlaying) windSFX.Play();
        }
        else
        {
            windEffects.enableEmission = false;
            windSFX.Stop();
        }
        
        //Turn Anims
        if(TurnAnim)
        {
            if(angle.x >= 0.7f)
            {
                mAnimator.SetBool("isRight", true);
                mAnimator.SetBool("isLeft", false);
            }
            if(angle.x <= -0.7f)
            {
                mAnimator.SetBool("isRight", false);
                mAnimator.SetBool("isLeft", true);
            }
        }
        if(!TurnAnim)
        {
            mAnimator.SetBool("isLeft", false);
            mAnimator.SetBool("isRight", false);
        }
    }

    public void Moving(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            isMoving = true;
            mAnimator.SetBool("isMoving", true);
            if(isBoosting) mAnimator.SetBool("isBoosting", true);
        }
        if(context.canceled)
        {
            isMoving = false;
            mAnimator.SetBool("isMoving", false);           
        }
        
    }
    public void Boosting(InputAction.CallbackContext context)
    {
        if(0 <= BoostGauge && isMoving && context.started)
        {
            isBoosting = true;
            mAnimator.SetBool("isBoosting", true);
        }
        if(context.canceled && isMoving)
        {
            isBoosting = false;
            mAnimator.SetBool("isBoosting", false);
        }
        if(0 >= BoostGauge)
        {
            isBoosting = false;
            mAnimator.SetBool("isBoosting", false);
        }
        if(!isMoving)
        {
            isBoosting = true;
            mAnimator.SetBool("isMoving", false);
        }
        if(context.canceled)
        {
            isBoosting = false;
            mAnimator.SetBool("isBoosting", false);
        }
    }
    public void Rotating(InputAction.CallbackContext context)
    {
        if(context.performed && isMoving && !isBoosting) TurnAnim = true;
        if(context.canceled)
        {
            mAnimator.SetBool("isLeft", false);
            mAnimator.SetBool("isRight", false);
            if(!isMoving) TurnAnim = false;
            if(isMoving && !isBoosting) TurnAnim = false;
        }
    }
}
