using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenRespawn : MonoBehaviour
{
    public bool needToRespawn;
    public int timeForRespawn;
    public float respawnTime;
    public GameObject startRing;
    private RingYellow start;
    public List<GameObject> respawnables;

    void Start()
    {
        start = startRing.GetComponent<RingYellow>();
        respawnTime = timeForRespawn;
    }
    void Update()
    {
        if(start.isCollected)
        {
            needToRespawn = true;
            respawnTime -= Time.deltaTime;
        }
        if(respawnTime <= 0)
        {
            needToRespawn = false;
            RespawnStuff();
        }
    }

    public void RespawnStuff()
    {
        respawnTime = timeForRespawn;
        for(var i = respawnables.Count - 1; i > -1; i--)
        {
            respawnables[i].GetComponent<RespawnScript>().Respawn();
        }
    }
}
