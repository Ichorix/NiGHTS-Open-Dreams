using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackleBody : MonoBehaviour
{
    public float randomX, randomY, _time, _speed;
    public AnimationCurve curve;
    public Vector3 currentPos, gotoPos;

    void Start()
    {
        currentPos = transform.position;
        gotoPos = transform.position;

        _speed = 0.8f;
    }
    void Update()
    {
        if(_time <= 1)
        {
            _time += Time.deltaTime * _speed;
        }
        else
        {
            randomX = Random.Range(-20, 20);
            randomX *= 0.1f;
            randomY = Random.Range(-20, 20);
            randomY *= 0.1f;
            
            _time = 0;
            RandomMovement();
        }

        transform.position = Vector3.Lerp(currentPos, gotoPos, curve.Evaluate(_time));
    }


    public void RandomMovement()
    {
        currentPos = transform.position;
        gotoPos = new Vector3(randomX, randomY, transform.position.z);
    }

    
}
