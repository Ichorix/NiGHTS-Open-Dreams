using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPlayerUI : MonoBehaviour
{
    public NPlayerOpenControl openPlayer;
    public NPlayerLevelFollow levelPlayer;
    public NPlayerScriptableObject _stats;

    public float chipReq = 50;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI chipText;
    public BoostBar boostBar;

    void Update()
    {
        scoreText.text = levelPlayer.currentScore.ToString();
        timeText.text = levelPlayer.levelTimeLeft.ToString();
        chipText.text = levelPlayer.currentChips.ToString() + " / " + chipReq.ToString();
        boostBar.SetBoost((int)_stats.boostGauge);
    }
}
