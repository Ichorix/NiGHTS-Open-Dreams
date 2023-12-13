using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveForce : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Ball"))
        {
            Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();

            otherRb.velocity = new Vector3(otherRb.velocity.x, 0, otherRb.velocity.z);
        }
    }
}
