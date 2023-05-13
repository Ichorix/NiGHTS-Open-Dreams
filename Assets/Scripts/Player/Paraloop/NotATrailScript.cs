using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotATrailScript : MonoBehaviour
{
    public float spawnTime;
    public GameObject trailObject;
    public List<GameObject> trailObjects;
    public int amountOfGameobjects;
    
    public bool isLevel;
    public GameObject player;
    

    void Start()
    {
        spawnTime = 0;
        amountOfGameobjects = 0;
    }
    void Update()
    {
        spawnTime += Time.deltaTime;
        if(spawnTime >= 0.05f)
        {
            spawnTime = 0;
            SpawnTrail();
        }
        if(isLevel)
        {
            transform.position = player.transform.position;
        }
    }

    public void SpawnTrail()
    {
        amountOfGameobjects++;
        GameObject newTrail = Instantiate(trailObject, transform.position, Quaternion.identity);
        trailObjects.Add(newTrail);

        for(var i = trailObjects.Count - 1; i > -1; i--)
        {
            if (trailObjects[i] == null)
            trailObjects.RemoveAt(i);
        }
    }
}
