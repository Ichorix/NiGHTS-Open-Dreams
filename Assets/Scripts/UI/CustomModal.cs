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
    public int islandNum;


    void Start()
    {
        this.gameObject.SetActive(false);
    }
    void OnEnable()
    {
        chipCount = newPlayerControl.chipAmount;

        timeBonus = chipCount * 0.25f;
        bonusNumber.text = timeBonus.ToString() + "s";
    }

    public void BossLevel()
    {
        Debug.Log("Boss");
        this.gameObject.SetActive(false);
        if(islandNum == 1)
        {
            Debug.Log("Growth");
            SceneManager.LoadScene("JackleFight");
        }
        if(islandNum == 2)
        {
            
        }
        if(islandNum == 3)
        {

        }
        if(islandNum == 4)
        {

        }
        if(islandNum == 5)
        {

        }
        else
        { Debug.Log("huh");}
    }
    public void DayLevel()
    {
        Debug.Log("Day");
        this.gameObject.SetActive(false);
        if(islandNum == 1)
        {
            Debug.Log("Growth Day");
            stateController.Activate3(0);
        }
        if(islandNum == 2)
        {
            
        }
        if(islandNum == 3)
        {

        }
        if(islandNum == 4)
        {

        }
        if(islandNum == 5)
        {

        }
        else
        { Debug.Log("huh");}
    }
    public void NightLevel()
    {
        Debug.Log("Night");
        this.gameObject.SetActive(false);
        if(islandNum == 1)
        {
            Debug.Log("Growth Night");
        }
        if(islandNum == 2)
        {
            
        }
        if(islandNum == 3)
        {

        }
        if(islandNum == 4)
        {

        }
        if(islandNum == 5)
        {

        }
        else
        { Debug.Log("huh");}
    }
    public void CancelStart()
    {
        Debug.Log("Canceled");
        this.gameObject.SetActive(false);
    }
}
