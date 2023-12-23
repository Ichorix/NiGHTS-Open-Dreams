using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class NPlayerStateController : MonoBehaviour
{

    [SerializeField] private bool gamePaused;
    public bool GamePaused
    {
        get{ return gamePaused; }
        set
        {
            gamePaused = value;
                openControl._speed = 0;
                openControl.enabled = !gamePaused;
                gameUI.SetActive(!gamePaused);
                MainMenuUI.SetActive(gamePaused);
                MainMenuCamera.SetActive(gamePaused);
                _input._stats = gamePaused ? null : _stats;
                AudioListener.pause = gamePaused;
                MainMenuNiGHTS.SetBool("GameOn", !gamePaused);
                MainMenuSkins.SetBool("GameOn", !gamePaused);
                ResetStats();
        }
    }
    public float UsableDeltaTime
    {
        get{ return gamePaused ? 0 : Time.deltaTime; }
    }
    public GameObject openPlayer;
    private NPlayerOpenControl openControl;
    public GameObject levelPlayer;
    private NPlayerLevelFollow levelFollow;
    private Camera mainCamera;
    [SerializeField] private GameObject MainMenuUI;
    [SerializeField] private GameObject MainMenuCamera;
    [SerializeField] private Animator MainMenuNiGHTS;
    [SerializeField] private Animator MainMenuSkins;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private NPlayerInput _input;
    [SerializeField] private NPlayerUI UIController;
    [SerializeField] private NPlayerScriptableObject _stats;
    [Space]
    [Header("Skin Management")]
    [SerializeField] private NPlayerSkinManager[] skinManagers;
    [SerializeField] private int selectedSkin;
    public int SetSkin
    {
        get
        {
            if(PlayerPrefs.HasKey("selectedSkin"))
                selectedSkin = PlayerPrefs.GetInt("selectedSkin");
            return selectedSkin;
        }
        set
        {
            selectedSkin = value;
            PlayerPrefs.SetInt("selectedSkin", selectedSkin);
            foreach(NPlayerSkinManager skinManager in skinManagers)
                skinManager.SwitchSkins(selectedSkin);
        }
    }

    void Awake()
    {
        mainCamera = Camera.main;
        openControl = openPlayer.transform.GetChild(0).GetComponent<NPlayerOpenControl>();
        levelFollow = levelPlayer.transform.GetChild(0).GetComponent<NPlayerLevelFollow>();
        GamePaused = GamePaused; // Sets the GamePaused state to whatever was defined in the inspector.
    }
    void Start()
    {
        // Activates the Open Player by default
        ActivateOpenPlayer();
    }

    public void ActivateOpenPlayer()
    {
        openPlayer.SetActive(true);
        levelPlayer.SetActive(false);
        _stats.isLevelPlayer = false;
        UIController.ActivateLevelUI(false);
        ResetStats();
    }

    public void ActivateLevelPlayer(PathCreator[] paths, float[] times, int[] chips, float extraTime)
    {
        NPlayerLevelFollow levelFollow = levelPlayer.transform.GetChild(0).GetComponent<NPlayerLevelFollow>();

        levelFollow.ActiveLevelPaths = paths;
        levelFollow.ActiveLevelTimes = times;
        levelFollow.ActiveLevelChipRequirement = chips;
        levelFollow.bonusTime = extraTime;

        // Activation has to be after the assignment so that the OnEnable() function assigns the rest of the values properly
        openPlayer.SetActive(false);
        levelPlayer.SetActive(true);
        _stats.isLevelPlayer = true;
        UIController.ActivateLevelUI(true);
        ResetStats();
    }

    public void ResetStats()
    {
        _stats.PowerBuffTimeLeft = 0;
        _stats.MoveDirection = Vector2.zero;
        _stats.MovementMultiplier = 0;
        _stats.TurningMultiplier = 1;
        _stats.isMoving = false;
        _stats.isBoosting = false;
    }
}
