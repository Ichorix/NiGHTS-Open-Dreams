using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPlayerAnimations : MonoBehaviour
{
    [SerializeField] private NPlayerOpenControl openControl;
    [SerializeField] private NPlayerLevelRotations levelRotations;
    
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _skinAnimator;
    [SerializeField] private NPlayerScriptableObject _stats;
    //Public so that they can be cleared by the paraloop
    public ParticleSystem rightHandSparkles;
    public ParticleSystem leftHandSparkles;
    [SerializeField] private TrailRenderer boostTrail;
    [SerializeField] private ParticleSystem boostParticles;
    [SerializeField] private ParticleSystem speedWindParticles;
    [SerializeField] private float speedWindSpeedThreshold = 30f;
    [SerializeField] private float turningAnimationThreshold = 0.6f;
    private bool boostAnim;
    private bool boostOverride;
    public bool Grounded;

    [Header("Sounds")]
    [SerializeField] private AudioSource boostingAudioSource;
    [SerializeField] private AudioClip boostingStartSFX;
    [SerializeField] private AudioClip boostingEndSFX;
    private bool boostingSoundsActive;
    

    void Update()
    {
        _animator.SetBool("isMoving", _stats.isMoving);
        _animator.SetBool("Grounded", Grounded);
        _skinAnimator.SetBool("isMoving", _stats.isMoving);
        _skinAnimator.SetBool("Grounded", Grounded);
        
        // Prevents the AnyState from transitioning to itself since the animation is split in half
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.LeftStep") ||
            _animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.RightStep"))
        {
            _animator.SetBool("DontTransitionToGrounded", true);
            _skinAnimator.SetBool("DontTransitionToGrounded", true);
        }
            
        else
        {
            _animator.SetBool("DontTransitionToGrounded", false);
            _skinAnimator.SetBool("DontTransitionToGrounded", false);
        }
            
        BoostStateManagement();

        BoostingAnimations();

        if(!_stats.isLevelPlayer)
        {
            NPlayerOpenControl_TurningAnimations();
            SpeedWindManagement();
        }
        else
        NPlayerLevelFollow_TurningAnimations();
    }

    private void BoostStateManagement()
    {
        if(_stats.isMoving)
        {
            boostAnim = false;
            if(_stats.BoostGauge > 0)
                boostAnim = _stats.isBoosting;
            else if(_stats.BoostAttempt)
                boostAnim = !Grounded;
            rightHandSparkles.enableEmission = true;
            leftHandSparkles.enableEmission = true;
        }
        else
        {
            boostAnim = false;
            rightHandSparkles.enableEmission = false;
            leftHandSparkles.enableEmission = false;
        }
    }

    // Used for the boost attempt
    public void BoostAnimationOverride(bool state)
    {
        boostAnim = state;
        boostOverride = state;
    }
    private void BoostingAnimations()
    {
        _animator.SetBool("isBoosting", boostAnim);
        _skinAnimator.SetBool("isBoosting", boostAnim);
        if(!boostOverride)
        {
            boostParticles.enableEmission = boostAnim;
            boostTrail.emitting = boostAnim;

            // Sounds placed here to not play when boost attempting
            if(boostAnim && !boostingSoundsActive)
                StartCoroutine(BoostingSounds());
        }
    }
    IEnumerator BoostingSounds()
    {
        boostingSoundsActive = true;
        boostingAudioSource.volume = 1f;
        boostingAudioSource.PlayOneShot(boostingStartSFX, 1f);
        boostingAudioSource.PlayDelayed(boostingStartSFX.length);
        while(boostAnim)
        {
            yield return null;
        }
        boostingSoundsActive = false;
        boostingAudioSource.Stop();
        boostingAudioSource.PlayOneShot(boostingEndSFX, 1f);
        yield break;
    }

    private void SpeedWindManagement()
    {
        speedWindParticles.enableEmission = openControl._speed > speedWindSpeedThreshold;
    }

    private void NPlayerOpenControl_TurningAnimations()
    {
        if(_stats.isMoving && _stats.MoveDirection != Vector2.zero)
        {
            _animator.SetBool("isRight", _stats.MoveDirection.x > turningAnimationThreshold);
            _animator.SetBool("isLeft", _stats.MoveDirection.x < -turningAnimationThreshold);
            _skinAnimator.SetBool("isRight", _stats.MoveDirection.x > turningAnimationThreshold);
            _skinAnimator.SetBool("isLeft", _stats.MoveDirection.x < -turningAnimationThreshold);
        }
        else
        {
            _animator.SetBool("isRight", false);
            _animator.SetBool("isLeft", false);
            _skinAnimator.SetBool("isRight", false);
            _skinAnimator.SetBool("isLeft", false);
        }
    }

    private void NPlayerLevelFollow_TurningAnimations()
    {
        _animator.SetBool("isUpsideDown", levelRotations.upsideDownTime > 0 && _stats.isMoving);
        _skinAnimator.SetBool("isUpsideDown", levelRotations.upsideDownTime > 0 && _stats.isMoving);
        
        if(_stats.MoveDirection.y > turningAnimationThreshold)
        {
            _animator.SetBool("isUp", true);
            _skinAnimator.SetBool("isUp", true);
        }
        else if(_stats.MoveDirection.y < -turningAnimationThreshold)
        {
            _animator.SetBool("isDown", true);
            _skinAnimator.SetBool("isDown", true);
        }
        else
        {
            _animator.SetBool("isUp", false);
            _animator.SetBool("isDown", false);
            _skinAnimator.SetBool("isUp", false);
            _skinAnimator.SetBool("isDown", false);
        }
    }
}
