using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    public ParaloopScript trailScript;
    public Vector3 collidedPosition, oppositePosition, centerPosition;
    public int collidedNum;
    public float otherNum;
    public GameObject instantiatedEffect;

    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("ParaloopTrail"))
        {
            collidedNum = trailScript.trailObjects.IndexOf(other.gameObject);
            otherNum = collidedNum * 0.5f;
            otherNum = (int)otherNum;

            collidedPosition = trailScript.trailObjects[collidedNum].transform.position;
            oppositePosition = trailScript.trailObjects[(int)otherNum].transform.position;

            centerPosition = (collidedPosition + oppositePosition) * 0.5f;

            Instantiate(instantiatedEffect, centerPosition, Quaternion.identity);
        }
    }
}
