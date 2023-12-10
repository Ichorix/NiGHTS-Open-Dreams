using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public EEPoint Enter;
    public EEPoint Exit;
    public bool disableMesh;
    public bool Outer;
    public bool allowDespawning;
    //public bool firstTime = true;
    public List<GameObject> respawnables;
    void Start()
    {
        if(disableMesh)
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Outer = true;
            //Debug.Log("Outer");
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && Outer)
        {
            if(Enter.active)
            {
                //Debug.Log("Entered Direction");
                Outer = false;
                RespawnStuff();
            }
            if(Exit.active)
            {
                //Debug.Log("Exited Direction");
                Outer = false;

                if(allowDespawning) DespawnStuff();
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        Outer = false;
    }

    public void RespawnStuff()
    {
        for(var i = respawnables.Count - 1; i > -1; i--)
        {
            if(respawnables[i].GetComponent<RespawnScript>() != null)
                respawnables[i].GetComponent<RespawnScript>().Respawn();
        }
    }
    public void DespawnStuff()
    {
        for(var i = respawnables.Count - 1; i > -1; i--)
        {
            if(respawnables[i].GetComponent<RespawnScript>() != null)
                respawnables[i].GetComponent<RespawnScript>().Despawn();
        }
    }
}
