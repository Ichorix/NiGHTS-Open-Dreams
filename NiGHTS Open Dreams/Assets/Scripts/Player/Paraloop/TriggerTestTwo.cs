using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTestTwo : MonoBehaviour
{
    [SerializeField] private NotATrailScript trailScript;
    [SerializeField] private GameObject playerReference;
    private Vector3 collidedPosition, oppositePosition, centerPosition;
    private int collidedNum;
    private float otherNum;
    [SerializeField] private GameObject instantiatedEffect;
    public GameObject instantiatedEffectInstance;
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("ParaloopTrail"))
        {
            collidedNum = trailScript.trailObjects.IndexOf(other.gameObject);
            
            if(collidedNum >= 0)
            {
                int totalObjects = trailScript.trailObjects.Count;
                
                otherNum = ((totalObjects - collidedNum) * 0.5f) + collidedNum;
                otherNum = (int)otherNum;

                collidedPosition = trailScript.trailObjects[collidedNum].transform.position;
                oppositePosition = trailScript.trailObjects[(int)otherNum].transform.position;

                centerPosition = (collidedPosition + oppositePosition) * 0.5f;


                // TODO Figure out how to spawn the paraloop but only once but without cooldown
                if(instantiatedEffectInstance == null)
                {
                    instantiatedEffectInstance = Instantiate(instantiatedEffect, centerPosition, Quaternion.identity);
                    instantiatedEffectInstance.GetComponent<ParaloopCollection>()._player = playerReference; 
                }

                trailScript.RemoveTrail();
            }
        }
    }
}
