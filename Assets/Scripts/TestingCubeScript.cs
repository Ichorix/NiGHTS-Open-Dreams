using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingCubeScript : MonoBehaviour
{
    public float speed;
    public Vector3 rotationVector;
    public Quaternion rotationQuaternion;
    void Update()
    {
        float playerVerticalInput = Input.GetAxis("Vertical");
        float playerHorizontalInput = Input.GetAxis("Horizontal");
        
        if(playerVerticalInput >= 0)
        {
            playerHorizontalInput *= -1;
        }
        if(playerVerticalInput == 0)
        {
            playerHorizontalInput *= 4;
            if(playerHorizontalInput <= 0)
            {
                playerHorizontalInput = 0;
            }
        }

        float playerInput = ((playerVerticalInput * 180) + (playerHorizontalInput * 90))/2;
        
        
        rotationVector.z = playerInput;
        rotationQuaternion.eulerAngles = rotationVector;
        transform.rotation = rotationQuaternion;
    }
}
