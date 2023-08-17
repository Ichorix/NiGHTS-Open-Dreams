using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObjects/PlayerStats")]
public class NPlayerScriptableObject : ScriptableObject
{   
    [Header("Inspector Variables")]
    [Tooltip("The speed that the player will rotate at"), Range(0, 500)]
    public float turningSpeed = 250;
    [Tooltip("The top speed that the player will reach normally"), Range(0, 100)] //Make sure to change the Clamp in NPlayerOpenControl if you increase past 100
    public float normalSpeed = 25;
    [Tooltip("The speed that the player will stay at after boosting while still moving"), Range(0, 100)]
    public float speedABoosting = 35;
    [Tooltip("The top speed that the player can reach while boosting"), Range(0, 100)]
    public float boostingSpeed = 75;
    [Tooltip("The rate of acceleration when not boosting"), Range(0, 100)]
    public float normalAccelerationRate = 10;
    [Tooltip("The rate of acceleration while boosting"), Range(0, 100)]
    public float boostingAccelerationRate = 20;
    [Tooltip("The rate that the player will decelerate at after the movement button has been released"), Range(0, 100)]
    public float decelerationRate = 50;
    [Tooltip("The speed that the player will recenter towards forward facing at"), Range(0, 100)]
    public float recenterSpeed = 3f;
    [Tooltip("The maximum boost that can be stored in the boostGauge"), Range(0, 200)]
    public float maxBoost = 100;
    [Tooltip("The players Current boost")]
    public float boostGauge;

    [Space]
    [Header("Input Values")]
    [Tooltip("The direction that the player should move in")]
    public Vector2 MoveDirection; 
    [Tooltip("Reads if the player should be moving forward or not")]
    public bool isMoving; 
    [Tooltip("Reads if the player should boost when moving or not")]
    public bool isBoosting;
    [Tooltip("Reads whether or not the camera should be attatched to the player or not")]
    public bool cameraPlayerBound;
    
}
