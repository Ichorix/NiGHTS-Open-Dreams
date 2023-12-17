using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    [SerializeField] private EEPoint Enter;
    [SerializeField] private EEPoint Exit;
    [Tooltip("Prevents the mesh from being disabled on start. Used for debugging")]
    [SerializeField] private bool dontDisableMesh;
    private bool Outer;
    [Tooltip("Allows you to make it so that this checkpoint despawns collectables when the player goes the wrong way")]
    [SerializeField] private bool allowDespawning;
    public List<GameObject> respawnables;
    void Start()
    {
        if(!dontDisableMesh)
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    void OnEnable()
    {
        RespawnStuff();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            Outer = true;
    }
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && Outer)
        {
            // Checks the left and right triggers to see which direction the player went through
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
