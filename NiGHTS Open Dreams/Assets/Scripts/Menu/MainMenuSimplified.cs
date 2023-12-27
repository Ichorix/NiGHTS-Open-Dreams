using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuSimplified : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private MenuMusicSimplified menuMusic;
    [SerializeField] private TextMeshProUGUI quitText;
    [SerializeField] private NPlayerStateController stateController;
    private NPlayerLevelFollow levelFollow;
    [SerializeField] private NPlayerScriptableObject _stats;

    void OnEnable()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        menuMusic.Main.Play();
        levelFollow = stateController.levelPlayer.transform.GetChild(0).GetComponent<NPlayerLevelFollow>();
        quitText.text = _stats.isLevelPlayer ? " Exit Level" : " Quit Game";
    }
    public void QuitGame()
    {
        if(_stats.isLevelPlayer)
        {
            Debug.Log("Exit Level");
            stateController.GamePaused = false;
            levelFollow.DoExitLevel();
        }
        else
        {
            Debug.Log("Quit Game");
            Application.Quit();
        }
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
