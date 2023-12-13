using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysZero : MonoBehaviour
{
    public Transform parent;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, parent.position, 1);
    }
}
