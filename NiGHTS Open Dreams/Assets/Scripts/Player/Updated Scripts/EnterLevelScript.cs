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
    public GameObject modalInstance;

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
                modalInstance.GetComponent<UIModalButtons>().Enable(_stats.openChips,
                            other.transform.parent.parent.GetComponent<NPlayerStateController>(),
                            Paths, thisStage);
            }
        }
    }
}
