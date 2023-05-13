using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeCloudStay : MonoBehaviour
{
    public Transform playerObject;
    public Vector3 playerPosition, setPosition, startPosition;
    public AnimationCurve curve;

    public float curveOutput;

    void Start()
    {
        startPosition = transform.position;
        setPosition = startPosition;
    }
    void Update()
    {
        playerPosition = playerObject.position;
        
        playerPosition.x = 0;
        playerPosition.z = 0;
        playerPosition.y -= 25;

        Debug.Log(curve.Evaluate((playerPosition.y * 0.02f)+ 0.5f));
        curveOutput = curve.Evaluate((playerPosition.y * 0.02f)+ 0.5f);

        curveOutput *= 20;

        setPosition.y = curveOutput - 218;

        transform.position = setPosition;
    }
}
