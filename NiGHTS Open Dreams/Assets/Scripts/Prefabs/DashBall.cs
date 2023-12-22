using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

public class DashBall : MonoBehaviour
{
    public float launchTime;
    private float timeLeft;
    public float launchSpeed;
    public float upSpeed;
    public ForceMode upForceMode;

    private NPlayerLevelFollow levelFollow;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");
        levelFollow = other.gameObject.GetComponent<NPlayerLevelFollow>();

        if(levelFollow != null)
            StartCoroutine(Launch());
    }

    // If youre getting a MoveNext() error in this Coroutine, I have absolutely no idea how to fix it
    IEnumerator Launch()
    {
        Debug.Log("Launch");
        levelFollow.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * upSpeed, upForceMode);
        timeLeft = launchTime;
        while (timeLeft > 0)
        {
            levelFollow.distanceTravelled += launchSpeed * Time.deltaTime;
            timeLeft -= Time.deltaTime;
            yield return null;
        }
    }
}
