using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingGreen : MonoBehaviour
{
    public bool isCollected;
    public float timeUntilGone;
    public Animator RingAnim;
    public ParticleSystem Front;
    public ParticleSystem Back;

    // I dont feel like fixing these .enableEmission, but if you feel so inclined check out NPlayerAnimations
    void Start()
    {
        isCollected = false;
        timeUntilGone = 1f;
        Front.enableEmission = true;
        Back.enableEmission = true;
    }

    void Update()
    {
        if(!isCollected)
        {
            Front.enableEmission = true;
            Back.enableEmission = true;
        }
        else
        {
            timeUntilGone -= Time.deltaTime;
            Front.enableEmission = false;
            Back.enableEmission = false;
        }
        if(timeUntilGone <= 0)
        {
            RingAnim.SetTrigger("TrReset");
            timeUntilGone = 1f;
            isCollected = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(!isCollected && other.CompareTag("levelPlayer"))
        {
            isCollected = true;
            RingAnim.SetTrigger("TrCollected");
        }
    }
}
