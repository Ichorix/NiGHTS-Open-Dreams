using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNights : MonoBehaviour
{
    private Animator mAnimator;
    private Rigidbody NightsRB;

    void Start()
    {
        mAnimator = GetComponent<Animator>();
        NightsRB = GetComponent<Rigidbody>();
        NightsRB.useGravity = false;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        if(other.gameObject.CompareTag("AnimStarter"))
        {
            Debug.Log("Correct");
            mAnimator.SetTrigger("TrStart");
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            StartFall();
        }
    }

    public void StartFall()
    {
        NightsRB.useGravity = true;
    }
}
