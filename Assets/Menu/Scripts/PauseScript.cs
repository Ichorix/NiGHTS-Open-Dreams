using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PauseScript : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    //pauseMenu.activeSelf
    PlayerInputActions controls;

    public Volume volume;
    private ColorAdjustments colorAdj;

    void Awake()
    {
        controls = new PlayerInputActions();
        controls.Player.Enable();
    }
    void OnEnable()
    {
        controls.Player.Enable();
    }
    void OnDisable()
    {
        controls.Player.Disable();
    }
    void Start()
    {
        volume.profile.TryGet(out colorAdj);
    }
    public void CheckPause()
    {
        Debug.Log("CheckPause");
        if(!isGamePaused) Pause();
        else{
            if(pauseMenu.activeSelf) Continue();
            else if(optionsMenu.activeSelf) Back();
            else isGamePaused = false;
        }
    }
    public void DebugLogIt()
    {
        Debug.Log("I was called");
    }

    public void Continue()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        colorAdj.active = false;
    }
    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        colorAdj.active = true;
    }

    public void MainMenu()
    {
        Debug.Log("Menu");
        Time.timeScale = 1f;
        colorAdj.active = false;
        SceneManager.LoadScene("MenusBasic");
    }

    public void Back()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
}
