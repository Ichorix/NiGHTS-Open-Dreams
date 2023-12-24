using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeRingBehaviour : MonoBehaviour
{
    [SerializeField] private CollectablesData data;
    [SerializeField] private float HitTime;
    [SerializeField] private float enterTime;
    [SerializeField] private bool hit;
    void OnEnable()
    {
        enterTime = 0;
        hit = false;
    }
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            enterTime += Time.fixedDeltaTime;
            if(enterTime >= HitTime)
            {
                if(!hit) HitPlayer(other);
                
            }
        }
    }

    void HitPlayer(Collider other)
    {
        hit = true;
        other.GetComponent<NPlayerCollisionController>().CollectItem(other, data);
        enterTime = 0;
    }
}
