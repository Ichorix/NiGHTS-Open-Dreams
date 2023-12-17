using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotATrailScript : MonoBehaviour
{
    [SerializeField] private Collider collider;
    [SerializeField] private NPlayerAnimations _animations;
    [SerializeField] private GameObject trailObject;
    [SerializeField] private float distanceThreshold;
    public List<GameObject> trailObjects;

    void OnEnable()
    {
        RemoveTrail();
        collider.enabled = true;
    }
    void OnDisable()
    {
        collider.enabled = false;
    }
    void Update()
    {
        if(trailObjects.Count <= 0)
        {
            SpawnTrail();
            return;
        }
        if(Vector3.Distance(transform.position, trailObjects[trailObjects.Count - 1].transform.position) >= distanceThreshold)
            SpawnTrail();
    }
    public void RemoveTrail()
    {
        for(var i = trailObjects.Count - 1; i > -1; i--)
        {
            Destroy(trailObjects[i]);
            trailObjects.RemoveAt(i);
            _animations.rightHandSparkles.Clear();
            _animations.leftHandSparkles.Clear();
        }
    }
    public void SpawnTrail()
    {
        GameObject newTrail = Instantiate(trailObject, transform.position, Quaternion.identity);
        trailObjects.Add(newTrail);

        for(var i = trailObjects.Count - 1; i > -1; i--)
        {
            if (trailObjects[i] == null)
            trailObjects.RemoveAt(i);
        }
    }
}
