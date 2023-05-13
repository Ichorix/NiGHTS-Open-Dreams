using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSimplified : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject playMenu;
    public MenuMusicSimplified menuMusicScript;

    void Start()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        playMenu.SetActive(false);
        menuMusicScript.Main.Play();
    }
    public void QuitGame()
    {
        Debug.Log("QuitGame");
        Application.Quit();
    }

    public void PlayClicked()
    {
        mainMenu.SetActive(false);
        playMenu.SetActive(true);
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

        while(!operation.isDone)
        {
            Debug.Log(operation.progress);

            yield return null;
        }
    }
}
