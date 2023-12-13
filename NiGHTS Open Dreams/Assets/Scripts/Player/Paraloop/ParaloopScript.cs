using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaloopScript : MonoBehaviour
{
    public float spawnTime;
    public GameObject trailObject;
    public List<GameObject> trailObjects;
    public int amountOfGameobjects;
    //public Vector3 spawnPos;

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

    //public ParaloopScript trailScript;
    public Vector3 collidedPosition, oppositePosition, centerPosition;
    public int collidedNum;
    public float otherNum;
    public GameObject instantiatedEffect;

    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("ParaloopTrail"))
        {
            collidedNum = trailObjects.IndexOf(other.gameObject);
            otherNum = collidedNum * 0.5f;
            otherNum = (int)otherNum;

            collidedPosition = trailObjects[collidedNum].transform.position;
            oppositePosition = trailObjects[(int)otherNum].transform.position;

            centerPosition = (collidedPosition + oppositePosition) * 0.5f;

            Instantiate(instantiatedEffect, centerPosition, Quaternion.identity);
        }
    }
}
