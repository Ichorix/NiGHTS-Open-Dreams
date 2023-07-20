using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class TutorialCameraPath : MonoBehaviour
{
    public TutorialLevelScript player;
    public PathCreator currentPath;
    public float distanceTravelled;
    public EndOfPathInstruction endOfPathInstruction;
    private Vector3 pathPosition;
    public Vector3 pathRotation;

    void Start()    ///Copied from the example project, probably useful to have
    {
        if (currentPath != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            currentPath.pathUpdated += OnPathChanged;
        }
    }
    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged() {
        distanceTravelled = currentPath.path.GetClosestDistanceAlongPath(transform.position);
    }

    void Update()
    {
        distanceTravelled = player.distanceTravelled;
        pathPosition = currentPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        transform.position = pathPosition;

        pathRotation = currentPath.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction).eulerAngles;
        transform.eulerAngles = pathRotation;
    }
}
