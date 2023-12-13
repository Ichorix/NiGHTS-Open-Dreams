using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesScript : MonoBehaviour
{
    public float moveSpeed;
    public float turnAngle;
    public Vector2 randomTimeBetweenChange;
    public Vector3 lookDirection;

    void Start()
    {
        StartCoroutine(CalculateChance());
        lookDirection = Vector3.zero;
    }
    void Update()
    {
        MoveForward();
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    void ChangeDirection()
    {
        float plusOrMinus = Random.Range(0,2);
        if(plusOrMinus == 0) plusOrMinus = -turnAngle;
        if(plusOrMinus == 1) plusOrMinus = turnAngle;
        lookDirection.y += plusOrMinus;
        transform.eulerAngles = lookDirection;
    }

    IEnumerator CalculateChance()
    {
        while (true)
        {
        float min = randomTimeBetweenChange.x * 10;
        float max = randomTimeBetweenChange.y * 10;
        float t = ((int)Random.Range(min, max)) * 0.1f;
        Debug.Log(t);
        yield return new WaitForSeconds(t);
        ChangeDirection();
        }
    }

}
