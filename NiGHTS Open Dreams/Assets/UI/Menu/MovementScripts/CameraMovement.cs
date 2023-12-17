using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed;
    //Starting Position 159 -3 550
    //Starting Rotation -15 12 0
    void Start()
    {
        speed = 1;
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }
}
