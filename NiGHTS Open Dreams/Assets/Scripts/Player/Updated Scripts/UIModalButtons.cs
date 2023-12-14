using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PathCreation;

public class UIModalButtons : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bonusTimeText;
    private NPlayerStateController _playerStates;
    private PathCreator[] pathsInstance = new PathCreator[4];
    private CustomStageScriptableObject stageInformation;

    public void Enable(int bonus, NPlayerStateController playerStates, PathCreator[] bakedPaths, CustomStageScriptableObject stageInfo)
    {
        bonusTimeText.text = "+"+ bonus.ToString() + " s";
        _playerStates = playerStates;
        pathsInstance = bakedPaths;
        stageInformation = stageInfo;
    }
    public void EnterStage()
    {
        _playerStates.ActivateLevelPlayer(pathsInstance, stageInformation.Times, stageInformation.Grades);
        Destroy(this.gameObject);
    }
    public void Cancel()
    {
        Destroy(this.gameObject);
    }
}
