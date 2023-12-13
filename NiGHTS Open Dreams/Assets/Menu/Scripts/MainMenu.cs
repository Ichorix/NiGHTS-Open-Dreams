using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject pressMenu;
    public GameObject optionsMenu;


    public void PlayGame()
    {
        Debug.Log("Loading...");
        SceneManager.LoadScene("Dreamtopia");
    }
    public void QuitGame()
    {
        Debug.Log("QuitGame");
        Application.Quit();
    }
    public void OptionsBack()
    {
        whatScreen = 3;
        optionsMenu.SetActive(false);
        menuMusicScript.Options.Stop();
        mainMenu.SetActive(true);
        menuMusicScript.Main.Play();
    }
    public void OptionsClicked()
    {
        whatScreen = 4;
        mainMenu.SetActive(false);
        menuMusicScript.Main.Stop();
        optionsMenu.SetActive(true);
        menuMusicScript.Options.Play();
        
    }


    void Start()
    {
        menuTime = 0;
        whatScreen = 1;
    }
    public float menuTime;
    public int whatScreen;
    public bool screensMenuScreen; //1
    public bool pressMenuScreen; //2
    public bool mainMenuScreen; //3
    public bool optionsMenuScreen; //4
    void Update()
    {
        menuTime += Time.deltaTime;
        if(menuTime <= 37)
        {
            whatScreen = 1;
        }
        if(menuTime <= 38 && menuTime >= 37)
        {
            whatScreen = 2;
            pressMenu.SetActive(true);
        }
        //AnyKey
        if(Input.anyKeyDown && whatScreen == 2)
        {
            AnyKeyScreenPressed();
        }
    }

    /*Menu Music*/
    public MenuMusic menuMusicScript;


    /*AnyKey*/




    public void AnyKeyScreenPressed()
    {
        //Application.LoadLevel("Scene Name");
        pressMenu.SetActive(false);
        menuMusicScript.Violin.Stop();
        mainMenu.SetActive(true);
        menuMusicScript.Main.Play();
        whatScreen = 3;
    }

}
