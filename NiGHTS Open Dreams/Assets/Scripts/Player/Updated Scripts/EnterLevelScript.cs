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
    public int SavedGrade = 0;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(_stats.isLevelPlayer)
            {
                //New Track
            }
            else
            {
                if(modalInstance == null) // Spawn a new modal if one isnt already up. Prevents having multiple overlapping
                    modalInstance = Instantiate(UIModal);
                NPlayerStateController playerStates = other.transform.parent.parent.GetComponent<NPlayerStateController>();
                playerStates.ResetStats();
                modalInstance.GetComponent<UIModalButtons>().Enable(_stats.openChips, playerStates, Paths, thisStage);

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
}
