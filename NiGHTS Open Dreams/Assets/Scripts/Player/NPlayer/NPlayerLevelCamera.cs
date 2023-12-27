using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class NPlayerLevelCamera : MonoBehaviour
{
    [SerializeField] private NPlayerLevelFollow _player;
    private PathCreator currentPath;
    private EndOfPathInstruction endOfPathInstruction;
    public float distanceTravelled;

    [Header("Y Easing")]
    [SerializeField] private float playerYvalue;
    [SerializeField] private float levelYvalue;
    [Tooltip("The position between the Player's Y value and the Level's Y value that the camera will be at. 0 follows Level, 1 follows Player"), Range(0, 1)] 
    public float yPercent = 0.6f;
    private float positionBetweenYs;
    //
    public Vector3 pathPosition;

    void UpdatePath()
    {
        currentPath = _player.currentPath;
        endOfPathInstruction = _player.endOfPathInstruction;
    }
    void Update()
    {
        UpdatePath();
        distanceTravelled = _player.distanceTravelled;

        pathPosition = currentPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);

        playerYvalue = _player.transform.position.y;
        levelYvalue = pathPosition.y;
        positionBetweenYs = Mathf.Lerp(levelYvalue, playerYvalue, yPercent);

        transform.position = new Vector3(pathPosition.x, positionBetweenYs, pathPosition.z);

        transform.rotation = currentPath.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
    }
}
