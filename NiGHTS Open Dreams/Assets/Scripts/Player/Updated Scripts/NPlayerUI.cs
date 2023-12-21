using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPlayerUI : MonoBehaviour
{
    [SerializeField] private bool isLevelPlayer;
    [SerializeField] private NPlayerOpenControl openControl;
    [SerializeField] private NPlayerLevelFollow levelFollow;
    [SerializeField] private NPlayerScriptableObject _stats;

    [Space]
    [SerializeField] private TextMeshProUGUI chipText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject scoreObject;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject timeObject;
    [SerializeField] private BoostBar boostBar;

    void Update()
    {
        boostBar.SetBoost((int)_stats.BoostGauge);

        if(_stats.isLevelPlayer)
        {
            scoreText.text = levelFollow.currentScore.ToString("n0");
            timeText.text = ((int)levelFollow.LevelTimeLeft).ToString();
            chipText.text = levelFollow.currentChips.ToString("n0")
                + " / " + levelFollow.ActiveLevelChipRequirement[levelFollow.levelSegment].ToString();
        }
        else
        chipText.text = openControl.openChips.ToString("n0");
    }
    
    public void ActivateLevelUI(bool active)
    {
        scoreObject.SetActive(active);
        timeObject.SetActive(active);
    }
}
