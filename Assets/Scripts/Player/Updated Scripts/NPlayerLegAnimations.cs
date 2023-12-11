using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPlayerLegAnimations : MonoBehaviour
{
    public Transform intendedTarget;
    public float animationSpeed;
    private float distanceApart;

    void Update()
    {
        distanceApart = Vector3.Distance(transform.position, intendedTarget.position);
        float step = animationSpeed * distanceApart * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, intendedTarget.position, step);
    }
}
