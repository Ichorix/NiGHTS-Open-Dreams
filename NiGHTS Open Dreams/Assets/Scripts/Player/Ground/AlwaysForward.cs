using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysForward : MonoBehaviour
{
    public GroundScript groundScript;

    void LateUpdate()
    {
        transform.position = groundScript.transform.position;
    }
}
