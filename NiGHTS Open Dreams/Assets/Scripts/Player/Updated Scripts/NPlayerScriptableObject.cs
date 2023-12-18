using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObjects/PlayerStats")]
public class NPlayerScriptableObject : ScriptableObject
{   
    [Header("Inspector Variables")]
    [Tooltip("Determines if the player is currently in a Level")]
    public bool isLevelPlayer = false;
    [Space]
    [Header("Open Player Values")]
    [Tooltip("The speed that the player will rotate at"), Range(0, 500)]
    public float turningSpeed = 125;
    [Tooltip("The top speed that the player will reach normally"), Range(0, 100)] //Make sure to change the Clamp in NPlayerOpenControl if you increase past 100
    public float normalSpeed = 15;
    [Tooltip("The speed that the player will stay at after boosting while still moving"), Range(0, 100)]
    public float speedABoosting = 25;
    [Tooltip("The top speed that the player can reach while boosting"), Range(0, 100)]
    public float boostingSpeed = 40;
    [Tooltip("The speed that the player will jump to when they boost while having no BoostGauge"), Range(0, 100)]
    public float boostAttemptSpeed = 20;
    [Tooltip("The speed that the player will recenter towards forward facing at"), Range(0, 100)]
    public float recenterSpeed = 3;
    [Tooltip("The speed that the player will adjust to the normals of the ground upon collision"), Range(0, 50)]
    public float groundAdjustSpeed = 10;
    [Tooltip("This value will be multiplied by the direction vector and added to the movement speed, but only for looking downwards")]
    public float downwardsMomentumMultiplier = 5;
    [Tooltip("A value of 0 will prevent you from going below the normal speed. A positive number will make you slow down, and a negative number will make you speed up")]
    public float upwardsMomentumMultiplier = 0;
    [Tooltip("Displays whether the Power buff is active or not")]

    [Space]
    public bool PowerBuff;
    [SerializeField] private float powerBuffTimeLeft;
    [Tooltip("Displays how much time is left on the Power Buff")]
    public float PowerBuffTimeLeft
    {
        get{ return powerBuffTimeLeft;}
        set
        {
            if(powerBuffTimeLeft <= 0) PowerBuff = false;
            else PowerBuff = true;
            powerBuffTimeLeft = value;
        }
    }
    
    [Space]
    [Header("Level Player Values")]
    [Tooltip("The top speed that the player will reach normally"), Range(0, 100)] //Make sure to change the Clamp in NPlayerOpenControl if you increase past 100
    public float normalSpeedLevel = 15;
    [Tooltip("The speed that the player will stay at after boosting while still moving"), Range(0, 100)]
    public float speedABoostingLevel = 17;
    [Tooltip("The top speed that the player can reach while boosting"), Range(0, 100)]
    public float boostingSpeedLevel = 28;
    [Tooltip("The speed that the player will jump to when they boost while having no BoostGauge"), Range(0, 100)]
    public float boostAttemptSpeedLevel = 28;
    [Tooltip("The speed that the player will flip to upright position in the Level"), Range(0, 100)]
    public float flipSpeed = 2;
    
    [Space]
    [Header("Both Players Values")]
    [Tooltip("The rate of acceleration when not boosting"), Range(0, 100)]
    public float normalAccelerationRate = 20;
    [Tooltip("The rate of acceleration while boosting"), Range(0, 100)]
    public float boostingAccelerationRate = 30;
    [Tooltip("The rate that the player will decelerate at after the movement button has been released"), Range(0, 100)]
    public float decelerationRate = 50;
    [Tooltip("How long the boost Attempt will last, in seconds"), Range(0, 10)]
    public float boostAttemptTime = 0.2f;
    [Tooltip("The time that it will take before you can do another boost Attempt, in seconds"), Range(0, 10)]
    public float boostAttemptCooldown = 0.25f;
    [Tooltip("The maximum boost that can be stored in BoostGauge"), Range(0, 200)]
    public float maxBoost = 100;
    [Tooltip("The rate at which the player will consume boost"), Range(0, 100)]
    public float boostDepletionRate = 20;
    private float boostGauge;
    [Tooltip("The players Current boost")]
    public float BoostGauge
    {
        get{ return boostGauge; }
        set
        {
            boostGauge = Mathf.Clamp(value, 0, maxBoost);
        }
    }


    [Space]
    [Header("Input Values")]
    [Tooltip("The direction that the player should move in")]
    public Vector2 MoveDirection; 
    [Tooltip("The direction that the camera should be moved towards")]
    public Vector2 LookDirection;
    [Tooltip("The value from the Trigger to multiply values by")]
    public float MovementMultiplier;
    [Tooltip("The value from the other trigger to multiply turning speed by")]
    public float TurningMultiplier;
    [Tooltip("Reads if the player should be moving forward or not")]
    public bool isMoving; 
    [Tooltip("Reads if the player should boost when moving or not")]
    public bool isBoosting;
    [Tooltip("Reads if the player had just pressed the boost button")]
    public bool runBoostAttempt;
    [Tooltip("Reads if the player is currently running the boostAttempt Coroutine")]
    public bool BoostAttempt;
    [Tooltip("Reads whether or not the camera should be attatched to the player or not")]
    public bool cameraPlayerBound;

    [Space]
    [Header("Save Data")]
    [Tooltip("Multiplier for mouse input to Camera")]
    public float MouseLookSensitivity = 1;
    [Tooltip("Multiplier for joystic input to Camera")]
    public float JoystickLookSensitivity = 1;

    public float displayScore;
    public float levelTime;
    public int openChips;    
}
