using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFollow : MonoBehaviour
{
    [SerializeField] private Transform follow;
    [SerializeField] private bool position;
    [SerializeField] private bool rotation;
    void Update()
    {
        if(position) transform.position = follow.position;
        if(rotation) transform.rotation = follow.rotation;
    }
}
