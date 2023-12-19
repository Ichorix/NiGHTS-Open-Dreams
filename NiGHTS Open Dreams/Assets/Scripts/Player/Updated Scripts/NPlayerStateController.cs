using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class NPlayerStateController : MonoBehaviour
{
    public GameObject openPlayer;
    public GameObject levelPlayer;
    [SerializeField] private NPlayerUI UIController;
    [SerializeField] private NPlayerScriptableObject _stats;

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
