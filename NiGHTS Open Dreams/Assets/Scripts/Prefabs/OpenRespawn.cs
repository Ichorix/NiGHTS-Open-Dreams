using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenRespawn : MonoBehaviour
{
    [SerializeField] private float timeForRespawn;
    [SerializeField] private float respawnTime;
    public float RespawnTime
    {
        get{ return respawnTime; }
        set
        {
            respawnTime = Mathf.Clamp(value, 0, timeForRespawn);
            if(respawnTime <= 0)
                RespawnStuff();
        }
    }
    [SerializeField] private RingYellow start;
    public List<GameObject> respawnables;

    void Start()
    {
        RespawnTime = timeForRespawn;
    }

    void Update()
    {
        if(start.isCollected)
            RespawnTime -= Time.deltaTime;
    }

    public void RespawnStuff()
    {
        RespawnTime = timeForRespawn;
        for(var i = respawnables.Count - 1; i > -1; i--)
        {
            respawnables[i].GetComponent<RespawnScript>().Respawn();
        }
    }
}
