using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class NPlayerMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuUI;
    [SerializeField] private GameObject MainMenuCamera;
    [SerializeField] private GameObject ModalCanvas;
    [SerializeField] private GameObject gameUI;
    
    [Space]
    [SerializeField] private NPlayerStateController _stateControl;
    [SerializeField] private bool gamePaused;
    public bool GamePaused
    {
        get { return gamePaused; }
        set
        {
            gamePaused = value;
            ModalCanvas.SetActive(!gamePaused);
            gameUI.SetActive(!gamePaused);
            MainMenuUI.SetActive(gamePaused);
            MainMenuCamera.SetActive(gamePaused);
            AudioListener.pause = gamePaused;
            MainMenuNiGHTS.SetBool("GameOn", !gamePaused);
            MainMenuSkins.SetBool("GameOn", !gamePaused);
            _stateControl._input._stats = gamePaused ? null : _stateControl._stats;
            _stateControl.openControl._speed = 0;
            _stateControl.openControl.enabled = !gamePaused;
            _stateControl.ResetStats();
        }
    }

    [Header("Skin Management")]
    [SerializeField] private Animator MainMenuNiGHTS; //Used for default skin
    [SerializeField] private Animator MainMenuSkins; //Used for the two classic skins

    void Awake()
    {
        GamePaused = GamePaused; // Sets the GamePaused state to whatever was defined in the inspector.
    }
}
