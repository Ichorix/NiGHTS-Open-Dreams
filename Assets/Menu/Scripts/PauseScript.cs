using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PauseScript : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    //pauseMenu.activeSelf

    public Volume volume;
    private ColorAdjustments colorAdj;

    void Start()
    {
        volume.profile.TryGet(out colorAdj);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isGamePaused) Pause();
            else{
                if(pauseMenu.activeSelf) Continue();
                if(optionsMenu.activeSelf) Back();
            }
            
        }
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
