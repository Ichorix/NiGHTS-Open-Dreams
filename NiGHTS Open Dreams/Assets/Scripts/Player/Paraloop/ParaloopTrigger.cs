using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaloopTrigger : MonoBehaviour
{
    [SerializeField] private ParaloopEmitter trailScript;
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
                
                if(instantiatedEffectInstance == null)
                {
                    instantiatedEffectInstance = Instantiate(instantiatedEffect, centerPosition, Quaternion.identity);
                    ParaloopCollection loopCol = instantiatedEffectInstance.GetComponent<ParaloopCollection>();
                    loopCol._player = playerReference;
                    loopCol._trigger = this;
                }

                trailScript.RemoveTrail();
            }
        }
    }
}
