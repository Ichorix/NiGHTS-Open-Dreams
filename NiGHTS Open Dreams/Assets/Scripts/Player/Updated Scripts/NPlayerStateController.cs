using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class NPlayerStateController : MonoBehaviour
{
    [SerializeField] private GameObject openPlayer;
    [SerializeField] private GameObject levelPlayer;
    [SerializeField] private NPlayerUI UIController;
    [SerializeField] private NPlayerScriptableObject _stats;

    void Start()
    {
        // Activates the Open Player by default
        ActivatePlayer(false);
    }

    void ActivatePlayer(bool isLevelPlayer)
    {
        openPlayer.SetActive(!isLevelPlayer);
        levelPlayer.SetActive(isLevelPlayer);
        _stats.isLevelPlayer = isLevelPlayer;
        UIController.ActivateLevelUI(isLevelPlayer);
    }
    public void ActivateLevelPlayer(PathCreator[] paths, float[] times, AnimationCurve[] grades)
    {
        NPlayerLevelFollow levelFollow = levelPlayer.transform.GetChild(0).GetComponent<NPlayerLevelFollow>();

        levelFollow.ActiveLevelPaths = paths;
        levelFollow.ActiveLevelTimes = times;
        levelFollow.ActiveLevelGrading = grades;

        // Activation has to be after the assignment so that the OnEnable() function assigns the rest of the values properly
        openPlayer.SetActive(false);
        levelPlayer.SetActive(true);
        _stats.isLevelPlayer = true;
        UIController.ActivateLevelUI(true);
    }
}
