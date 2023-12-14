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
        //Activates the Open Player by default
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
        openPlayer.SetActive(false);
        levelPlayer.SetActive(true);
        _stats.isLevelPlayer = true;
        UIController.ActivateLevelUI(true);

        NPlayerLevelFollow levelFollow = levelPlayer.GetComponent<NPlayerLevelFollow>();
        Debug.Log("Got to here");
        levelFollow.ActiveLevelPaths = paths;
        Debug.Log("1");
        levelFollow.ActiveLevelTimes = times;
        Debug.Log("2");
        levelFollow.ActiveLevelGrading = grades;
        Debug.Log(":)");
    }
}
