using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPlayerSkinManager : MonoBehaviour
{
    [SerializeField] private GameObject skinBundle;
    [SerializeField] private GameObject classicHead;
    [SerializeField] private GameObject christmasHead;
    [SerializeField] private SkinnedMeshRenderer bodyMesh;
    [SerializeField] private Material classicBodyMat;
    [SerializeField] private Material christmasBodyMat;
    [Space]
    [SerializeField] private GameObject[] modernMeshes;

    void Start()
    {
        int selectedSkin = 0;
        if(PlayerPrefs.HasKey("selectedSkin"))
                selectedSkin = PlayerPrefs.GetInt("selectedSkin");
        SwitchSkins(selectedSkin);
    }

    public void SwitchSkins(int chosenSkin)
    {
        switch (chosenSkin)
        {
            case 0: // Modern
                Debug.Log("Modern");
                foreach(GameObject mesh in modernMeshes)
                    mesh.SetActive(true);
                skinBundle.SetActive(false);
                break;
            case 1: // Classic
                Debug.Log("Classic");
                foreach(GameObject mesh in modernMeshes)
                    mesh.SetActive(false);
                skinBundle.SetActive(true);

                bodyMesh.material = classicBodyMat;
                classicHead.SetActive(true);
                christmasHead.SetActive(false);
                break;
            case 2: // Christmas
                Debug.Log("Christmas");
                foreach(GameObject mesh in modernMeshes)
                    mesh.SetActive(false);
                skinBundle.SetActive(true);

                bodyMesh.material = christmasBodyMat;
                classicHead.SetActive(false);
                christmasHead.SetActive(true);
                break;
        }
    }
}
