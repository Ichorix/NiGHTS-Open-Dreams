using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class EnterLevelScript : MonoBehaviour
{
    [SerializeField] private NPlayerScriptableObject _stats;
    [SerializeField] private GameObject asscoiatedOpenLevel;
    [SerializeField] private Transform[] ideyaDestinations = new Transform[4];
    [SerializeField] private IdeyaCapture[] ideyaCaptures = new IdeyaCapture[4];
    public PathCreator[] Paths = new PathCreator[4];
    public float[] ActiveAttemptScores = new float[4];
    public int [] ActiveAttemptGrades = new int[4];
    [SerializeField] private CustomStageScriptableObject thisStage;
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

                        thisStage.SavedGrade = fullGrade > thisStage.SavedGrade ? fullGrade : thisStage.SavedGrade;
                        thisStage.SavedScore = fullScore > thisStage.SavedScore ? fullScore : thisStage.SavedScore;

                        StartCoroutine(levelFollow.BeatLevel());
                    }
                }
            }
            else
            {
                asscoiatedOpenLevel.SetActive(true);
                if(modalInstance == null) // Spawn a new modal if one isnt already up. Prevents having multiple overlapping
                    modalInstance = Instantiate(UIModal);

                NPlayerStateController playerStates = other.transform.parent.parent.GetComponent<NPlayerStateController>();
                NPlayerLevelFollow levelFollow = playerStates.levelPlayer.transform.GetChild(0).GetComponent<NPlayerLevelFollow>();
                
                playerStates.ResetStats();
                levelFollow.ActiveLevelPalace = this;

                modalInstance.GetComponent<UIModalButtons>().Enable(_stats.openChips, playerStates, Paths, thisStage);

                scoreSpinner.SetInteger("Grade", thisStage.SavedGrade);
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
            Debug.Log(i);
            ideyaCaptures[i].ReturnMyIdeya();
        }
    }
}
