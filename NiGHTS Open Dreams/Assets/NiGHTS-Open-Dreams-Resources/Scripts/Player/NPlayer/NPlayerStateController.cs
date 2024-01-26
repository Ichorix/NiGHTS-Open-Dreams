using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class NPlayerStateController : MonoBehaviour
{
    public NPlayerMenuManager menuManager;
    public float UsableDeltaTime
    {
        get{ return menuManager.GamePaused ? 0 : Time.deltaTime; }
    }
    public GameObject openPlayer;
    [HideInInspector] public NPlayerOpenControl openControl;
    public GameObject levelPlayer;
    [HideInInspector] public NPlayerLevelFollow levelFollow;
    [HideInInspector] public Camera mainCamera;
    
    public NPlayerInput _input;
    public NPlayerUI UIController;
    public NPlayerScriptableObject _stats;

    void Awake()
    {
        openControl = openPlayer.transform.GetChild(0).GetComponent<NPlayerOpenControl>();
        levelFollow = levelPlayer.transform.GetChild(0).GetComponent<NPlayerLevelFollow>();
        mainCamera = Camera.main;
    }
    void Start()
    {
        // Activates the Open Player by default
        ActivateOpenPlayer();
    }

    public void ActivateOpenPlayer()
    {
        openPlayer.SetActive(true);
        levelPlayer.SetActive(false);
        _stats.isLevelPlayer = false;
        UIController.ActivateLevelUI(false);
        ResetStats();
    }

    // Called from the Enter Level Modal in UIModalButtons.EnterStage()
    public void ActivateLevelPlayer(PathCreator[] paths, float[] times, int[] chips, float extraTime)
    {
        NPlayerLevelFollow levelFollow = levelPlayer.transform.GetChild(0).GetComponent<NPlayerLevelFollow>();

        levelFollow.ActiveLevelPaths = paths;
        levelFollow.ActiveLevelTimes = times;
        levelFollow.ActiveLevelChipRequirement = chips;
        levelFollow.bonusTime = extraTime;

        // Activation has to be after the assignment so that the OnEnable() function assigns the rest of the values properly
        openPlayer.SetActive(false);
        levelPlayer.SetActive(true);
        _stats.isLevelPlayer = true;
        UIController.ActivateLevelUI(true);
        ResetStats();
    }

    public void ResetStats()
    {
        _stats.PowerBuffTimeLeft = 0;
        _stats.MoveDirection = Vector2.zero;
        _stats.MovementMultiplier = 0;
        _stats.TurningMultiplier = 1;
        _stats.isMoving = false;
        _stats.isBoosting = false;
    }
}
