using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTestTwo : MonoBehaviour
{
    public NotATrailScript trailScript;
    public float cooldownLength;
    public float cooldownTime;
    public Vector3 collidedPosition, oppositePosition, centerPosition;
    public int collidedNum;
    public float otherNum;
    public GameObject instantiatedEffect;

    void Update()
    {
        cooldownTime += Time.deltaTime;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("ParaloopTrail") && cooldownTime > cooldownLength)
        {
            collidedNum = trailScript.trailObjects.IndexOf(other.gameObject);
            
            int totalObjects = trailScript.trailObjects.Count;
            
            otherNum = ((totalObjects - collidedNum) * 0.5f) + collidedNum;
            otherNum = (int)otherNum;

            collidedPosition = trailScript.trailObjects[collidedNum].transform.position;
            oppositePosition = trailScript.trailObjects[(int)otherNum].transform.position;

            centerPosition = (collidedPosition + oppositePosition) * 0.5f;

            Instantiate(instantiatedEffect, centerPosition, Quaternion.identity);
            cooldownTime = 0;
            //Debug.Log("Paraloop; Total:" + totalObjects + "Collided: " + collidedNum + " at " + collidedPosition + " Other: " + otherNum + " at " + oppositePosition + " Midpoint: " + centerPosition);
        }
    }
    
}
