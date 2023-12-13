using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using PathCreation;

public class LevelFollowUsingPhysics : MonoBehaviour
{
    public PathCreator currentPath;
    public Rigidbody rigidbody;
    public float _speed;
    public float _turningSpeed;
    public bool isMoving;

    void Update()
    {
        float playerVerticalInput = Input.GetAxis("Vertical");
        float playerHorizontalInput = Input.GetAxis("Horizontal");

        if(playerHorizontalInput != 0 || playerVerticalInput != 0) isMoving = true;
        else isMoving = false;

        if(isMoving) rigidbody.AddForce(transform.forward * _speed);
        rigidbody.AddTorque(transform.right * _turningSpeed * playerVerticalInput);
    }
}
