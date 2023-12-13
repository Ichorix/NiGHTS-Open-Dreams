using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelePlayerPortal : MonoBehaviour
{
    public GameObject portalPrefab;
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("TriggerEnter");
        if(other.CompareTag("Player"))
        {
            Vector3 spawnPortalPos = new Vector3(transform.position.x, other.gameObject.transform.position.y, transform.position.z);
            Instantiate(portalPrefab, spawnPortalPos, Quaternion.identity);
        }
        if(other.CompareTag("JHands"))
        {
            other.gameObject.GetComponent<JackleHands>().leftRight *= -1;
        }
        
    }
}
