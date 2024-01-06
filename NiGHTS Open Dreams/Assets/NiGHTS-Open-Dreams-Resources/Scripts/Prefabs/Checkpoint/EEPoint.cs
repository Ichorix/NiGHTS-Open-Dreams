using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EEPoint : MonoBehaviour
{
    public bool active;
    void OnTriggerEnter()
    {
        active = true;
    }
    void OnTriggerExit()
    {
        active = false;
    }
}
