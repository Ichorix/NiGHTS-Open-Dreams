using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public EEPoint Enter;
    public EEPoint Exit;
    public bool Outer;
    public bool allowDespawning;
    //public bool firstTime = true;
    public List<GameObject> respawnables;
    void Start()
    {
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    void OnTriggerEnter()
    {
        Outer = true;
        Debug.Log("Outer");
    }
    void OnTriggerStay()
    {
        if(Outer)
        {
            if(Enter.active)
            {
                Debug.Log("Entered Direction");
                Outer = false;
                RespawnStuff();
            }
            if(Exit.active)
            {
                Debug.Log("Exited Direction");
                Outer = false;

                if(allowDespawning) DespawnStuff();
            }
        }
        //firstTime = false;
    }
    void OnTriggerExit()
    {
        Outer = false;
    }

    public void RespawnStuff()
    {
        for(var i = respawnables.Count - 1; i > -1; i--)
        {
            respawnables[i].GetComponent<RespawnScript>().Respawn();
        }
    }
    public void DespawnStuff()
    {
        for(var i = respawnables.Count - 1; i > -1; i--)
        {
            respawnables[i].GetComponent<RespawnScript>().Despawn();
        }
    }
}
