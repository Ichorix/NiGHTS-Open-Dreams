using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    public float forceMult = 0;
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Ball"))
        {
            Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();

            otherRb.velocity = new Vector3(otherRb.velocity.x * forceMult, otherRb.velocity.y * forceMult, otherRb.velocity.z * forceMult);
        }
    }
}
