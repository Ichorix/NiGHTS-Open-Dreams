using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public bool isCollectable, collect;
    public Animator mAnimator;

    void Update()
    {
        if(collect)
        {
            Collect();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(isCollectable)
        {
            Collect();
        }
    }

    void Collect()
    {
        //mAnimator.SetTrigger("CollectAnim");
    }
}
