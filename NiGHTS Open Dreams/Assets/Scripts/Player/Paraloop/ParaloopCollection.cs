using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaloopCollection : MonoBehaviour
{
    public GameObject _player;
    public ParaloopTrigger _trigger;
    // Triggers once per item that it collides with, allowing us to simply detect for the tag rather than detect from the collectable
    void OnTriggerStay(Collider other)
    {
        // This effectively removes it just a couple frames after colliding so that it isnt instantiated multiple times, yet still allows you to paraloop without cooldown
        if(_player != null)
            _trigger.instantiatedEffectInstance = null;

        if(other.CompareTag("ParaloopCollectable"))
        {
            CollectablesData item = other.GetComponent<Collectable>().data;
            if(item != null && _player != null)
            {
                _player.GetComponent<NPlayerCollisionController>().CollectItem(other, item);
            }
        }
    }
}
