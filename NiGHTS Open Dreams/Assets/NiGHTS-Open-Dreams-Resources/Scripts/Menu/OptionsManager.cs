using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private GameObject keybindingsMenu;
    [SerializeField] private GameObject othersMenu;
    [SerializeField] private GameObject displayMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private Button keybindsB;
    [SerializeField] private Button displayB;
    [SerializeField] private Button audioB;
    [SerializeField] private Button accessB;
    [Space]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundsSlider;
    [SerializeField] private Slider boostSlider;
    [SerializeField] private Slider ambienceSlider;
    [SerializeField] private TMPro.TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMPro.TMP_Dropdown skinsDropdown;
    Resolution[] resolutions;

    public NPlayerMasterSkinManager masterSkinManager;
    void Start()
    {
        Debug.Log("Options Start");
        GetResolutionOptions();
        LoadAudioValues();
        LoadSelectedSkin();
        Debug.Log("Options Loaded");
    }
    private void GetResolutionOptions()
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
        othersMenu.SetActive(false);
        displayMenu.SetActive(false);
        audioMenu.SetActive(false);

        keybindsB.interactable = false;
        displayB.interactable = true;
        audioB.interactable = true;
        accessB.interactable = true;
    }
    public void Display()
    {
        keybindingsMenu.SetActive(false);
        othersMenu.SetActive(false);
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
        othersMenu.SetActive(false);
        displayMenu.SetActive(false);
        audioMenu.SetActive(true);

        keybindsB.interactable = true;
        displayB.interactable = true;
        audioB.interactable = false;
        accessB.interactable = true;
    }
    public void Other()
    {
        keybindingsMenu.SetActive(false);
        othersMenu.SetActive(true);
        displayMenu.SetActive(false);
        audioMenu.SetActive(false);

        keybindsB.interactable = true;
        displayB.interactable = true;
        audioB.interactable = true;
        accessB.interactable = false;
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

    //////////Audio//////////
    private void LoadAudioValues()
    {
        if(PlayerPrefs.HasKey("masterVolume"))
            masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
        SetMasterVolume(masterSlider.value);

        if(PlayerPrefs.HasKey("musicVolume"))
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume(musicSlider.value);

        if(PlayerPrefs.HasKey("sfxVolume"))
            soundsSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        SetFXVolume(soundsSlider.value);

        if(PlayerPrefs.HasKey("boostVolume"))
            boostSlider.value = PlayerPrefs.GetFloat("boostVolume");
        SetBoostVolume(boostSlider.value);

        if(PlayerPrefs.HasKey("ambienceVolume"))
            ambienceSlider.value = PlayerPrefs.GetFloat("ambienceVolume");
        SetAmbienceVolume(ambienceSlider.value);
    }
    public void SetMasterVolume(float volumeValue)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(volumeValue) * 20);
        PlayerPrefs.SetFloat("masterVolume", volumeValue);
    }
    public void SetMusicVolume(float volumeValue)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volumeValue) * 20);
        PlayerPrefs.SetFloat("musicVolume", volumeValue);
    }
    public void SetFXVolume(float volumeValue)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(volumeValue) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volumeValue);
    }
    public void SetBoostVolume(float volumeValue)
    {
        audioMixer.SetFloat("boostVolume", Mathf.Log10(volumeValue) * 20);
        PlayerPrefs.SetFloat("boostVolume", volumeValue);
    }
    public void SetAmbienceVolume(float volumeValue)
    {
        audioMixer.SetFloat("ambienceVolume", Mathf.Log10(volumeValue) * 20);
        PlayerPrefs.SetFloat("ambienceVolume", volumeValue);
    }

    //////////Other//////////
    void LoadSelectedSkin()
    {
        int selectedSkin = 0;
        if(PlayerPrefs.HasKey("selectedSkin"))
                selectedSkin = PlayerPrefs.GetInt("selectedSkin");
        skinsDropdown.value = selectedSkin;
        Debug.Log("Set Skin Dropdown");
        SetSkin(selectedSkin);
    }
    public void SetSkin(int index)
    {
        Debug.Log(index);
        masterSkinManager.SetSkin = index;
    }
}
