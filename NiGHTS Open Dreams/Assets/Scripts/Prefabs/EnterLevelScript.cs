using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class EnterLevelScript : MonoBehaviour
{
    [Header("Save Data")]
    [Tooltip("The highest Grade that has been saved for this level")]
    [SerializeField] private int savedGrade = 0;
    public int SavedGrade
    {
        get
        {
            if(PlayerPrefs.HasKey("Stage1-1 Grade"))
                savedGrade = PlayerPrefs.GetInt("Stage1-1 Grade");
            return savedGrade;
        }
        set
        {
            savedGrade = value;
            PlayerPrefs.SetInt("Stage1-1 Grade", savedGrade);
        }
    }
    [Tooltip("The highest Score that has been saved for this level")]
    [SerializeField] private float savedScore = 0;
    public float SavedScore
    {
        get
        {
            if(PlayerPrefs.HasKey("Stage1-1 Score"))
                savedScore = PlayerPrefs.GetFloat("Stage1-1 Score");
            return savedScore;
        }
        set
        {
            savedScore = value;
            PlayerPrefs.SetFloat("Stage1-1 Score", savedScore);
        }
    }

    [Space]
    [Header("Level Information")]
    [SerializeField] private NPlayerScriptableObject _stats;
    [SerializeField] private CustomStageScriptableObject thisStage;
    [SerializeField] private GameObject asscoiatedOpenLevel;
    [SerializeField] private GameObject associatedLevelMusic;
    [SerializeField] private Transform[] ideyaDestinations = new Transform[4];
    [SerializeField] private IdeyaCapture[] ideyaCaptures = new IdeyaCapture[4];
    public PathCreator[] Paths = new PathCreator[4];
    public float[] ActiveAttemptScores = new float[4];
    public int [] ActiveAttemptGrades = new int[4];
    
    [Space]
    [Header("Instances")]
    [SerializeField] private GameObject ModalCanvas;
    [SerializeField] private GameObject UIModal;
    private GameObject modalInstance;
    [SerializeField] private Animator scoreSpinner;
    private GameObject scoreSpinnerInstance;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(_stats.isLevelPlayer)
            {
                // First spawn
                NPlayerLevelFollow levelFollow = other.GetComponent<NPlayerLevelFollow>();
                levelFollow.ActiveLevelPalace = this;
                // Since the levelPlayer triggers the palace when it first spawns, turn off the level here
                asscoiatedOpenLevel.SetActive(false);
                associatedLevelMusic.SetActive(true);

                // Continue Level
                if(levelFollow.ContinueLevel)
                {
                    IdeyaChase ideya = levelFollow.recoveredIdeya;
                    ideya.inPlace = true;
                    ideya.goToPosition = ideyaDestinations[levelFollow.levelSegment];

                    
                    ActiveAttemptScores[levelFollow.levelSegment] = levelFollow.currentScore;
                    ActiveAttemptGrades[levelFollow.levelSegment] = (int)(thisStage.Grades[levelFollow.levelSegment].Evaluate(levelFollow.currentScore));
                    
                    other.GetComponent<InstantiatePointItem>().InstantiateUItem(4);
                    scoreSpinner.SetInteger("Grade", ActiveAttemptGrades[levelFollow.levelSegment]);
                    scoreSpinner.SetTrigger("RunAnimation");

                    levelFollow.levelSegment++;
                    levelFollow.ContinueLevel = false;
                    if(levelFollow.levelSegment >= Paths.Length)
                    {
                        // Calculate and Save Final Grades
                        int sumGrade = 0;
                        foreach(int item in ActiveAttemptGrades)
                            sumGrade += item;
                        int fullGrade = (int)((sumGrade + 2) * 0.25f);

                        float fullScore = 0;
                        foreach(float item in ActiveAttemptScores)
                            fullScore += item;

                        SavedGrade = fullGrade > SavedGrade ? fullGrade : SavedGrade;
                        SavedScore = fullScore > SavedScore ? fullScore : SavedScore;

                        StartCoroutine(levelFollow.BeatLevel());
                    }
                }
            }
            else
            {
                asscoiatedOpenLevel.SetActive(true);
                associatedLevelMusic.SetActive(false);
                
                if(modalInstance == null) // Spawn a new modal if one isnt already up. Prevents having multiple overlapping
                    modalInstance = Instantiate(UIModal, ModalCanvas.transform);

                // Get the appropriate references from the player
                NPlayerStateController playerStates = other.transform.parent.parent.GetComponent<NPlayerStateController>();
                NPlayerLevelFollow levelFollow = playerStates.levelPlayer.transform.GetChild(0).GetComponent<NPlayerLevelFollow>();
                NPlayerOpenControl openControl = other.GetComponent<NPlayerOpenControl>();

                levelFollow.ActiveLevelPalace = this;

                // Stop the players movement 
                playerStates.ResetStats();

                // Send the information to the modal instance through UIModalButtons.Enable()
                modalInstance.GetComponent<UIModalButtons>().Enable(openControl.OpenChips, playerStates, Paths, thisStage, SavedScore);

                // Activate the animation
                scoreSpinner.SetInteger("Grade", SavedGrade);
                scoreSpinner.SetTrigger("RunAnimation");
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(_stats.isLevelPlayer)
            {
                //Unsure
            }
            else
            {
                Destroy(modalInstance);
            }
        }
    }

    public void ResetIdeyas()
    {
        for(int i = 0; i < ideyaCaptures.Length; i++)
        {
            ideyaCaptures[i].ReturnMyIdeya();
        }
    }
}
