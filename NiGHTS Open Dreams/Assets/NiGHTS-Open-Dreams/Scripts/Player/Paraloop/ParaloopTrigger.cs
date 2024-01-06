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
            
            // Weird errors can happen if you manage to collide with a negative value
            if(collidedNum >= 0)
            {
                // Calculate where the paraloop needs to spawn based on the locations provided by the objects in the trail
                int totalObjects = trailScript.trailObjects.Count;
                
                otherNum = ((totalObjects - collidedNum) * 0.5f) + collidedNum;
                otherNum = (int)otherNum;
                
                collidedPosition = trailScript.trailObjects[collidedNum].transform.position;
                oppositePosition = trailScript.trailObjects[(int)otherNum].transform.position;

                // Calculates the midpoint of the two points which is at least very close to the center
                centerPosition = (collidedPosition + oppositePosition) * 0.5f;
                
                // Spawn a new paraloop so long as there isn't one out already. The reference is removed about a frame after collecting the items inside the paraloop
                if(instantiatedEffectInstance == null)
                {
                    // Instantiate the paraloop and get a reference to the instance
                    instantiatedEffectInstance = Instantiate(instantiatedEffect, centerPosition, Quaternion.identity);
                    ParaloopCollection loopCol = instantiatedEffectInstance.GetComponent<ParaloopCollection>();
                    // Assign proper references to the instance of the paraloop
                    loopCol._player = playerReference;
                    loopCol._trigger = this;
                }
                // Remove the trail objects to let the next paraloop happen
                trailScript.RemoveTrail();
            }
        }
    }
}
