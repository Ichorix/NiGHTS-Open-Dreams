using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicAnimationScript : MonoBehaviour
{
    public Animator mAnimator;
    void Start()
    {
        mAnimator = GetComponent<Animator>();
    }

    
    /*****EXAMLES*****\
    mAnimator.SetTrigger("Trigger");
    mAnimator.ResetTrigger("Trigger");
    mAnimator.SetBool("Boolean", value);
    mAnimator.SetInteger("Int", 1);
    *///End
}
