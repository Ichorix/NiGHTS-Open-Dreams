using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSimplified : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public MenuMusicSimplified menuMusic;

    void OnEnable()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        menuMusic.Main.Play();
    }
    public void QuitGame()
    {
        Debug.Log("QuitGame");
        Application.Quit();
    }

    public void OptionsClicked()
    {
        mainMenu.SetActive(false);
        menuMusic.Main.Stop();
        optionsMenu.SetActive(true);
        menuMusic.Options.Play();
    }
    public void OptionsBack()
    {
        optionsMenu.SetActive(false);
        menuMusic.Options.Stop();
        mainMenu.SetActive(true);
        menuMusic.Main.Play();
    }
}
