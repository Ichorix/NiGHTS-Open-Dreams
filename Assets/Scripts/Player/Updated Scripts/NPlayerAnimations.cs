using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPlayerAnimations : MonoBehaviour
{
    [SerializeField] private bool isLevelPlayer;
    public NPlayerLevelRotations levelPlayer;
    
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private NPlayerScriptableObject _stats;
    [SerializeField]
    private TrailRenderer boostTrail;
    [SerializeField]
    private ParticleSystem boostParticles;
    [SerializeField]
    private float turningAnimationThreshold = 0.6f;

    private bool boostAnim;
    private bool boostOverride;

    void Update()
    {
        _animator.SetBool("isMoving", _stats.isMoving);
        BoostStateManagement();

        BoostingAnimations();

        if(!isLevelPlayer)
        NPlayerOpenControl_TurningAnimations();
        else
        NPlayerLevelFollow_TurningAnimations();
    }
    private void BoostStateManagement()
    {
        if(_stats.isMoving)
        {
            if(_stats.boostGauge > 0)
                boostAnim = _stats.isBoosting;
            else
            if(_stats.runBoostAttempt)
                boostAnim = _stats.isBoosting;
        }
        else
            boostAnim = false;
    }
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
