using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPlayerUI : MonoBehaviour
{
    [SerializeField] private bool isLevelPlayer;
    [SerializeField] private NPlayerOpenControl openPlayer;
    [SerializeField] private NPlayerLevelFollow levelPlayer;
    [SerializeField] private NPlayerScriptableObject _stats;

    public float chipReq = 50;
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
            scoreText.text = levelPlayer.currentScore.ToString();
            timeText.text = ((int)levelPlayer.levelTimeLeft).ToString();
            chipText.text = levelPlayer.currentChips.ToString() + " / " + chipReq.ToString();
        }
        else
        chipText.text = _stats.openChips.ToString();
    }
    
    public void ActivateLevelUI(bool active)
    {
        scoreObject.SetActive(active);
        timeObject.SetActive(active);
    }
}
