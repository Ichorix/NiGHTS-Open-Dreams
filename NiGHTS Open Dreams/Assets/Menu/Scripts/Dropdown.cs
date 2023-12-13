using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dropdown : MonoBehaviour
{
    public int output;
    public GameObject playStation, ninSwitch, xBox;
    public bool b_playStation, b_ninSwitch, b_xBox;

    public void HandleInputData(int val)
    {
        if(val == 0)
        {
            PlayStation();
        }
        if(val == 1)
        {
            NinSwitch();
        }
        if(val == 2)
        {
            Xbox();
        }
    }

    public void Keyboard()
    {
        output = 0;
    }
    public void GamePadCheck()
    {
        if(output == 1)
        {
            b_playStation = true;
            b_ninSwitch = false;
            b_xBox = false;
        }
        if(output == 2)
        {
            b_playStation = false;
            b_ninSwitch = true;
            b_xBox = false;
        }
        if(output == 3)
        {
            b_playStation = false;
            b_ninSwitch = false;
            b_xBox = true;
        }
    }
    void PlayStation()
    {
        playStation.SetActive(true);
        ninSwitch.SetActive(false);
        xBox.SetActive(false);

        output = 1;
    }
    void NinSwitch()
    {
        playStation.SetActive(false);
        ninSwitch.SetActive(true);
        xBox.SetActive(false);

        output = 2;
    }
    void Xbox()
    {
        playStation.SetActive(false);
        ninSwitch.SetActive(false);
        xBox.SetActive(true);

        output = 3;
    }

}
