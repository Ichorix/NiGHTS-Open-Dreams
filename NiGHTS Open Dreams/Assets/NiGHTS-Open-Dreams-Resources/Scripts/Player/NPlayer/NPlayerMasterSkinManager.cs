using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class NPlayerMasterSkinManager : MonoBehaviour
{
    [SerializeField] private NPlayerSkinManager[] skinManagers;
    [SerializeField] private int selectedSkin;
    public int SetSkin
    {
        get
        {
            if (PlayerPrefs.HasKey("selectedSkin"))
                selectedSkin = PlayerPrefs.GetInt("selectedSkin");
            return selectedSkin;
        }
        set
        {
            selectedSkin = value;
            PlayerPrefs.SetInt("selectedSkin", selectedSkin);
            foreach (NPlayerSkinManager skinManager in skinManagers)
                skinManager.SwitchSkins(selectedSkin);
        }
    }
}
