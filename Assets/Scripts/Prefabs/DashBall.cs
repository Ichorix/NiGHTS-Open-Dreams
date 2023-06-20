using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

public class DashBall : MonoBehaviour
{
    public float launchTime;
    private float time;
    public float launchSpeed;
    public float upSpeed;
    public float step;
    
    public LevelFollow player;
    public LevelAnims anim;
    private Rigidbody rb;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("levelPlayer"))
        {
            player = other.gameObject.GetComponent<LevelFollow>();
            anim = other.gameObject.GetComponent<LevelAnims>();
            StartCoroutine(Launch());
            rb = other.gameObject.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * upSpeed);
        }
    }

    IEnumerator Launch()
    {
        anim.mAnimator.SetBool("isBoosting", true);
        time = launchTime;
        while (time > 0)
        {
            player.distanceTravelled += launchSpeed;
            time -= step;
            yield return new WaitForSeconds(step);
        }//mAnimator.SetBool("isBoosting", false);
    }
}
