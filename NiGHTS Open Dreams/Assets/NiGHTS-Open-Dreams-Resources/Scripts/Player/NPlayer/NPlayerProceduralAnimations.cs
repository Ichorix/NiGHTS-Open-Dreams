using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPlayerProceduralAnimations : MonoBehaviour
{
    public Transform intendedTarget;
    public float animationSpeed;
    public float gravityDifference;

    void Update()
    {
        // Always moves the targets towards the intended position by a speed, faster if the target has further to move
        float distanceApart = Vector3.Distance(transform.position, intendedTarget.position);
        float step = animationSpeed * distanceApart * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, intendedTarget.position + new Vector3(0, gravityDifference, 0), step);
    }
}
