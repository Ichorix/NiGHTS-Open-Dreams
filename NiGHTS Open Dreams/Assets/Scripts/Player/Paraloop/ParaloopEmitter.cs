using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaloopEmitter : MonoBehaviour
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

    // This version of the ParaloopEmitter is an updated version of the one shown in the video.
    // Instead of having a time between spawns like before that could change in density based on your movement speed,
    // The new version now calculates the distance from the last object to see if it is far enough away based on the distanceThreshold
    // Vector3.Distance is actually pretty fast so it probably isn't much of a performance difference compared to incrementing time
    void Update()
    {
        // If there is no trail object currently in the list, then spawn the trail and don't do the rest of the logic
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
