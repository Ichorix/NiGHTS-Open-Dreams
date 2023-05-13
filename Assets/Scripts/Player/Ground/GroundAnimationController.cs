using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAnimationController : MonoBehaviour
{
    private GroundScript groundScript;
    [SerializeField] GameObject player;
    private Animator mAnimator;
    
    void Awake()
    {
        groundScript = player.GetComponent<GroundScript>();
    }
    void Start()
    {
        mAnimator = GetComponent<Animator>();
    }

    public void RunningAnimation()
    {
        mAnimator.ResetTrigger("TrIdle");
        mAnimator.SetTrigger("TrRunning");
        Debug.Log("Running");
    }
    public void JumpingAnimation()
    {
        mAnimator.SetTrigger("TrJump");
        Debug.Log("Jumping");
    }
    public void IdleAnimation()
    {
        mAnimator.ResetTrigger("TrRunning");
        mAnimator.SetTrigger("TrIdle");
        Debug.Log("Idle");
    }
}
