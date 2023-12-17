using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaloopCollection : MonoBehaviour
{
    public GameObject _player;
    // Triggers once per item that it collides with, allowing us to simply detect for the tag rather than detect from the collectable
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("BlueChip"))
        {
            Debug.Log("I was a blue Chip");
            if(_player != null)
                _player.GetComponent<NPlayerCollisionController>().CollectBlueChip(other);
            else
                Debug.Log("It wasnt added Fast enough!!");
            return;
        }
        if(other.CompareTag("Star"))
        {
            Debug.Log("I was a star chip");
            if(_player != null)
                _player.GetComponent<NPlayerCollisionController>().CollectStarChip(other);
            else
                Debug.Log("It wasnt added Fast enough!!");
            return;
        }
    }
}
