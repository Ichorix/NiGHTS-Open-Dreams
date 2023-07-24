using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSimplified : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject playMenu;
    public MenuMusicSimplified menuMusicScript;

    void Start()
    {
        if(mainMenu != null)
        {
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
            playMenu.SetActive(false);
            menuMusicScript.Main.Play();
        }
        if(loadingScreen != null)
            loadingScreen.SetActive(false);
    }
    public void QuitGame()
    {
        Debug.Log("QuitGame");
        Application.Quit();
    }

    public void PlayClicked()
    {
        Debug.Log("Loading... as NiGHTS");
        StartCoroutine(LoadingGameAsync());
        /*
        mainMenu.SetActive(false);
        playMenu.SetActive(true);
        */
    }
    public void PlayBack()
    {
        playMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void PlayGame(int playType)
    {
        if(playType == 1)
        {
            Debug.Log("Loading... as NiGHTS");
            StartCoroutine(LoadingGameAsync());

        }
        if(playType == 2)
        {
            Debug.Log("Loading... as Sonic");
            //SceneManager.LoadScene("TerrainMaker");
        }
        if(playType == 3)
        {
            Debug.Log("Loading... as NiGHTS and Sonic");
            //SceneManager.LoadScene("TerrainMaker");
        }
    }
    

    public void OptionsClicked()
    {
        mainMenu.SetActive(false);
        menuMusicScript.Main.Stop();
        optionsMenu.SetActive(true);
        menuMusicScript.Options.Play();
    }
    public void OptionsBack()
    {
        optionsMenu.SetActive(false);
        menuMusicScript.Options.Stop();
        mainMenu.SetActive(true);
        menuMusicScript.Main.Play();
    }
    IEnumerator LoadingGameAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("TerrainMaker");
        if(loadingScreen != null) loadingScreen.SetActive(true);
        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            if(slider != null) slider.value = progress;

            yield return null;
        }
    }
}
