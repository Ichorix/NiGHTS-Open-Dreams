using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class NPlayerAnimations : MonoBehaviour
{
    [SerializeField] private NPlayerOpenControl openControl;
    [SerializeField] private NPlayerLevelRotations levelRotations;
    
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _skinAnimator;
    [SerializeField] private NPlayerProceduralAnimations legTarget;
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

    void OnDisable()
    {
        boostingAudioSource.Stop();
    }

    // Sets the values for both animators at the same time
    // Could be optimized by having the references, and an _animator variable that is set in OnEnable() to the correct animator
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
        var rHand = rightHandSparkles.emission;
        var lHand = leftHandSparkles.emission;

        if(_stats.isMoving)
        {
            boostAnim = false;
            if(_stats.BoostGauge > 0)
                boostAnim = _stats.isBoosting;
            else if(_stats.BoostAttempt)
                boostAnim = !Grounded;

            rHand.enabled = true;
            lHand.enabled = true;
        }
        else
        {
            boostAnim = false;
            rHand.enabled = false;
            lHand.enabled = false;
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
        var boostStars = boostParticles.emission;
        _animator.SetBool("isBoosting", boostAnim);
        _skinAnimator.SetBool("isBoosting", boostAnim);
        legTarget.animationSpeed = boostAnim ? 100 : 5;
        if(!boostOverride)
        {
            boostStars.enabled = boostAnim;
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
        // Plays the boosting start sound
        boostingAudioSource.PlayOneShot(boostingStartSFX, 1f);
        // Then when the boosting start sound is over, play the audioSource
        // This will play the BoostContinue sound and loop it since that is what is defined in the audioSource
        boostingAudioSource.PlayDelayed(boostingStartSFX.length);
        
        // So long as you are boosting, keep doing that
        while(boostAnim)
            yield return null;

        // When you stop boosting then stop the sound effects and play the boostEnd sound
        boostingSoundsActive = false;
        boostingAudioSource.Stop();
        boostingAudioSource.PlayOneShot(boostingEndSFX, 1f);
        yield break;
    }

    private void SpeedWindManagement()
    {
        var windFX = speedWindParticles.emission;
        windFX.enabled = openControl._speed > speedWindSpeedThreshold;
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
