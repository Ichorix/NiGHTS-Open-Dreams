using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CustomModal : MonoBehaviour
{
    public NewPlayerControl newPlayerControl;
    public StateController stateController;
    public TextMeshProUGUI bonusNumber;
    public int chipCount;
    public float timeBonus;


    void Start()
    {
        this.gameObject.SetActive(false);
    }
    void OnEnable()
    {
        timeBonus = newPlayerControl.chipAmount;
        bonusNumber.text = timeBonus.ToString() + "s";
    }

    public void BossLevel()
    {
        Debug.Log("Boss");
        this.gameObject.SetActive(false);

        Debug.Log("Growth");
        SceneManager.LoadScene("JackleFight");
    }
    public void DayLevel()
    {
        Debug.Log("Day");
        this.gameObject.SetActive(false);
        Debug.Log("Growth Day");
        stateController.Activate3(0);
    }
    public void NightLevel()
    {
        Debug.Log("Night");
        this.gameObject.SetActive(false);
        Debug.Log("Growth Night");
    }
    public void CancelStart()
    {
        Debug.Log("Canceled");
        this.gameObject.SetActive(false);
    }
}
