using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PathCreation;

public class UIModalButtons : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bonusTimeText;
    [SerializeField] private TextMeshProUGUI HighScoreText;
    private NPlayerStateController _playerStates;
    private PathCreator[] pathsInstance = new PathCreator[4];
    private CustomStageScriptableObject stageInformation;
    private float bonusTime;

    public void Enable(int bonus, NPlayerStateController playerStates, PathCreator[] bakedPaths, CustomStageScriptableObject stageInfo, float savedScore)
    {
        bonusTime = Mathf.Clamp(bonus, 0, 60);
        bonusTimeText.text = "+"+ bonusTime.ToString() + " s";
        _playerStates = playerStates;
        pathsInstance = bakedPaths;
        stageInformation = stageInfo;
        HighScoreText.text = savedScore.ToString("n0");
    }
    public void EnterStage()
    {
        _playerStates.ActivateLevelPlayer(pathsInstance, stageInformation.Times, stageInformation.ChipsRequired, bonusTime);
        Destroy(this.gameObject);
    }
    public void Cancel()
    {
        Destroy(this.gameObject);
    }
}
