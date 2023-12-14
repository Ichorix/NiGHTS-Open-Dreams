using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
