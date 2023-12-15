using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPlayerAnimations : MonoBehaviour
{
    [SerializeField] private NPlayerOpenControl openPlayer;
    [SerializeField] private NPlayerLevelRotations levelPlayer;
    
    [SerializeField] private Animator _animator;
    [SerializeField] private NPlayerScriptableObject _stats;
    [SerializeField] private ParticleSystem rightHandSparkles;
    [SerializeField] private ParticleSystem leftHandSparkles;
    [SerializeField] private TrailRenderer boostTrail;
    [SerializeField] private ParticleSystem boostParticles;
    [SerializeField] private float turningAnimationThreshold = 0.6f;

    private bool boostAnim;
    private bool boostOverride;
    public bool Grounded;
    

    void Update()
    {
        _animator.SetBool("isMoving", _stats.isMoving);
        _animator.SetBool("Grounded", Grounded);
        
        // Prevents the AnyState from transitioning to itself since the animation is split in half
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.LeftStep") ||
            _animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.RightStep"))
            _animator.SetBool("DontTransitionToGrounded", true);
        else _animator.SetBool("DontTransitionToGrounded", false);
            
        BoostStateManagement();

        BoostingAnimations();

        if(!_stats.isLevelPlayer)
        NPlayerOpenControl_TurningAnimations();
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
        if(!boostOverride)
        {
            boostParticles.enableEmission = boostAnim;
            boostTrail.emitting = boostAnim;
        }
    }

    private void NPlayerOpenControl_TurningAnimations()
    {
        if(_stats.isMoving && _stats.MoveDirection != Vector2.zero)
        {
            if(_stats.MoveDirection.x > turningAnimationThreshold)
                _animator.SetBool("isRight", true);
            else
                _animator.SetBool("isRight", false);

            if(_stats.MoveDirection.x < -turningAnimationThreshold)
                _animator.SetBool("isLeft", true);
            else 
                _animator.SetBool("isLeft", false);
        }
        else
        {
            _animator.SetBool("isRight", false);
            _animator.SetBool("isLeft", false);
        }
    }

    private void NPlayerLevelFollow_TurningAnimations()
    {
        if(levelPlayer.upsideDownTime > 0 && _stats.isMoving)
            _animator.SetBool("isUpsideDown", true);
        else
            _animator.SetBool("isUpsideDown", false);
        
        if(_stats.MoveDirection.y > turningAnimationThreshold)
            _animator.SetBool("isUp", true);
        else if(_stats.MoveDirection.y < -turningAnimationThreshold)
            _animator.SetBool("isDown", true);
        else
        {
            _animator.SetBool("isUp", false);
            _animator.SetBool("isDown", false);
        }
    }
}
