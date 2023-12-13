using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPassthrough : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ball"))
        {
            int rand = Random.Range(0,4);
            Debug.Log(rand);
            if(rand >= 2)
            {
                Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
                otherRb.velocity = Vector3.zero;
            }
        }
    }
}
