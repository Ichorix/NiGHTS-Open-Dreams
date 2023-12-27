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
        switch(forceDirection)
        {
            case 0:
                direction = Vector3.up;
                break;
            case 1:
                direction = Vector3.down;
                break;
            case 2:
                direction = Vector3.left;
                break;
            case 3:
                direction = Vector3.right;
                break;
            case 4:
                direction = Vector3.forward;
                break;
            case 5:
                direction = Vector3.back;
                break;
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
        Rigidbody forcedBody = other.GetComponent<Rigidbody>();
        if(other.tag != "Ground" && other.tag != "unForcable" && forcedBody != null)
        {
            forcedBody.AddForce(direction * force);
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
