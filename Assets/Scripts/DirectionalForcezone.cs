using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalForcezone : MonoBehaviour
{
    [Header("Inspector Variables")]
    [SerializeField, Tooltip("0: up, 1: down, 2: left, 3: right, 4: forward, 5: back")]
    private int forceDirection;

    [SerializeField, Tooltip("0: off, 1: small, 2: medium, 3: large")]
    private int forcePower;
    [Space]
    public float smallForce = 75;
    public float mediumForce = 100;
    public float largeForce = 150;

    [Header("Debug & Misc")]
    public bool forceEnabled;
    [SerializeField] private float force;
    [SerializeField] private Vector3 direction;

    void Start()
    {
        
        if(forceDirection == 0)
        {
            direction = Vector3.up;
        }
        if(forceDirection == 1)
        {
            direction = Vector3.down;
        }
        if(forceDirection == 2)
        {
            direction = Vector3.left;
        }
        if(forceDirection == 3)
        {
            direction = Vector3.right;
        }
        if(forceDirection == 4)
        {
            direction = Vector3.forward;
        }
        if(forceDirection == 5)
        {
            direction = Vector3.back;
        }

        if(forcePower == 0)
        {
            force = 0;
        }
        if(forcePower == 1)
        {
            force = smallForce;
        }
        if(forcePower == 2)
        {
            force = mediumForce;
        }
        if(forcePower == 3)
        {
            force = largeForce;
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        if(other.tag != "Ground" && other.tag != "unForcable" && other.GetComponent<Rigidbody>() != null)
        {
            other.GetComponent<Rigidbody>().AddForce(direction * force);
        }
    }

    public void EnableForce(float newForce)
    {
        forceEnabled = true;
        force = newForce;
    }
    public void DisableForce()
    {
        forceEnabled = false;
        force = 0;
    }
}
