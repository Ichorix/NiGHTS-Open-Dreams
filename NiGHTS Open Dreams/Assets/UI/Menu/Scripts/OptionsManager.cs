using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class OptionsManager : MonoBehaviour
{
    public GameObject keybindingsMenu;
    public GameObject accessibilitiesMenu;
    public GameObject displayMenu;
    public GameObject audioMenu;
    public Button keybindsB;
    public Button displayB;
    public Button audioB;
    public Button accessB;

    public AudioMixer audioMixer;
    public Text masterAudioText;
    public TMPro.TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    public CinemachineFreeLook cameraSettings;
    public Button playerSpace;
    public Button worldSpace;

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        

        List<string> options = new List<string>();

        int currentResolution = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolution;
        resolutionDropdown.RefreshShownValue();
    }

    public void Keybindings()
    {
        keybindingsMenu.SetActive(true);
        accessibilitiesMenu.SetActive(false);
        displayMenu.SetActive(false);
        audioMenu.SetActive(false);

        keybindsB.interactable = false;
        displayB.interactable = true;
        audioB.interactable = true;
        accessB.interactable = true;
    }
    public void Accessibilities()
    {
        keybindingsMenu.SetActive(false);
        accessibilitiesMenu.SetActive(true);
        displayMenu.SetActive(false);
        audioMenu.SetActive(false);

        keybindsB.interactable = true;
        displayB.interactable = true;
        audioB.interactable = true;
        accessB.interactable = false;
    }
    public void Display()
    {
        keybindingsMenu.SetActive(false);
        accessibilitiesMenu.SetActive(false);
        displayMenu.SetActive(true);
        audioMenu.SetActive(false);

        keybindsB.interactable = true;
        displayB.interactable = false;
        audioB.interactable = true;
        accessB.interactable = true;
    }
    public void Audio()
    {
        keybindingsMenu.SetActive(false);
        accessibilitiesMenu.SetActive(false);
        displayMenu.SetActive(false);
        audioMenu.SetActive(true);

        keybindsB.interactable = true;
        displayB.interactable = true;
        audioB.interactable = false;
        accessB.interactable = true;
    }

    //////////Display//////////
    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
        Debug.Log("Quality is " + quality);
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Debug.Log("Fullscreen is " + isFullscreen);
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution setResolution = resolutions[resolutionIndex];
        Screen.SetResolution(setResolution.width, setResolution.height, Screen.fullScreen);
    }

    //////////Audio////////////
    public void SetMasterVolume(float volumeValue)
    {
        audioMixer.SetFloat("masterVolume", volumeValue);
    }
    public void SetMusicVolume(float volumeValue)
    {
        audioMixer.SetFloat("musicVolume", volumeValue);
    }
    public void SetFXVolume(float volumeValue)
    {
        audioMixer.SetFloat("sfxVolume", volumeValue);
    }
    public void SetAmbienceVolume(float volumeValue)
    {
        audioMixer.SetFloat("ambienceVolume", volumeValue);
    }

    //////////Accessibilities//
    public void CamSetWorld()
    {
        if(cameraSettings != null)
        {
            cameraSettings.m_BindingMode = CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;
            playerSpace.interactable = true;
            worldSpace.interactable = false;
        }
    }
    public void CamSetPlayer()
    {
        if(cameraSettings != null)
        {
            cameraSettings.m_BindingMode = CinemachineTransposer.BindingMode.LockToTarget;
            playerSpace.interactable = false;
            worldSpace.interactable = true;
        }
    }
    public void SetYInverted(bool YInvert)
    {
        if(cameraSettings != null)
        {
            cameraSettings.m_YAxis = new AxisState(0, 1, false, true, 2f, 0.2f, 0.1f, "Mouse Y", YInvert);
            Debug.Log("Y axis inversion is " + YInvert);
        }
    }
    public void SetXInverted(bool XInvert)
    {
        if(cameraSettings != null)
        {
            cameraSettings.m_XAxis = new AxisState(-180, 180, true, false, 300f, 0.1f, 0.1f, "Mouse X", XInvert);
            Debug.Log("X axis inversion is " + XInvert);
        }
    }

}
