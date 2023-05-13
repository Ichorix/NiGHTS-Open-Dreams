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
    void Start()
    {
        isCollected = false;
        timeUntilGone = 1f;
        Front.enableEmission = true;
        Back.enableEmission = true;
    }

    void Update()
    {
        if(isCollected == false)
        {
            Front.enableEmission = true;
            Back.enableEmission = true;
        }
        if(isCollected == true)
        {
            timeUntilGone -= Time.deltaTime;
            Front.enableEmission = false;
            Back.enableEmission = false;
        }
        if(timeUntilGone <= 0)
        {
            //Destroy(gameObject);
            //Debug.Log("");
            RingAnim.SetTrigger("TrReset");
            timeUntilGone = 1f;
            isCollected = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(isCollected == false && other.CompareTag("levelPlayer"))
        {
            isCollected = true;
            RingAnim.SetTrigger("TrCollected");
        }
    }
}
