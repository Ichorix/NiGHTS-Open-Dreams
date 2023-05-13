using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OpenLevelUI : MonoBehaviour
{
    public GameObject gPopup;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        gPopup.SetActive(true);
        
    }
}

