using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class EnterLevelScript : MonoBehaviour
{
    [SerializeField] private NPlayerScriptableObject _stats;
    public PathCreator[] Paths = new PathCreator[4];
    [SerializeField] private CustomStageScriptableObject thisStage;
    [SerializeField] private GameObject UIModal;
    private GameObject modalInstance;
    [SerializeField] private Animator scoreSpinner;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(_stats.isLevelPlayer)
            {
                NPlayerLevelFollow levelFollow = other.GetComponent<NPlayerLevelFollow>();
                if(levelFollow.ContinueLevel)
                {
                    // TODO Fix the grade check to incorporate all tracks
                    thisStage.SavedScore = levelFollow.currentScore > thisStage.SavedScore ? levelFollow.currentScore : thisStage.SavedScore;
                    int grade = (int)thisStage.Grades[levelFollow.levelSegment].Evaluate(levelFollow.currentScore);
                    thisStage.SavedGrade = grade > thisStage.SavedGrade ? grade : thisStage.SavedGrade;

                    levelFollow.levelSegment++;
                    levelFollow.ContinueLevel = false;
                }
            }
            else
            {
                if(modalInstance == null) // Spawn a new modal if one isnt already up. Prevents having multiple overlapping
                    modalInstance = Instantiate(UIModal);
                NPlayerStateController playerStates = other.transform.parent.parent.GetComponent<NPlayerStateController>();
                playerStates.ResetStats();
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
}
