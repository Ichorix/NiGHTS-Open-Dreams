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

    [SerializeField] private NPlayerLevelFollow levelFollow;

    void OnTriggerEnter(Collider other)
    {
        //levelFollow = other.gameObject.GetComponent<NPlayerLevelFollow>();

        if(levelFollow != null)
            StartCoroutine(Launch());
    }

    IEnumerator Launch()
    {
        if(upSpeed != 0)
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
