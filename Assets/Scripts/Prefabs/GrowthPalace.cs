using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;


public class GrowthPalace : MonoBehaviour
{
    public Material blueChipMat;
    public int freedIdeas;
    public bool got5thIdeya;
    public bool accessBossFight;
    public GameObject ideyaLocation1;
    public IdeyaFollow ideya1;
    public GameObject ideyaLocation2;
    public IdeyaFollow ideya2;
    public GameObject ideyaLocation3;
    public IdeyaFollow ideya3;
    public GameObject ideyaLocation4;
    public IdeyaFollow ideya4;
    public GameObject ideyaLocation5;
    public GameObject vfxBeam;

    
    void Start()
    {
        UpdateStuff();
        vfxBeam.SetActive(false);
    }

    public void UpdateStuff()
    {
        if(freedIdeas == 0)
         {
            ideyaLocation1.GetComponent<LineRenderer>().enabled = false;
            ideyaLocation2.GetComponent<LineRenderer>().enabled = false;
            ideyaLocation3.GetComponent<LineRenderer>().enabled = false;
            ideyaLocation4.GetComponent<LineRenderer>().enabled = false;
            accessBossFight = false;
        }
        else
        {
            if(freedIdeas >= 1) ideyaLocation1.GetComponent<LineRenderer>().enabled = true;
            if(freedIdeas >= 2) ideyaLocation2.GetComponent<LineRenderer>().enabled = true;
            if(freedIdeas >= 3) ideyaLocation3.GetComponent<LineRenderer>().enabled = true;
            if(freedIdeas >= 4)
            {
                ideyaLocation4.GetComponent<LineRenderer>().enabled = true;
                accessBossFight = true;
            }
        }
        if(accessBossFight)
            vfxBeam.SetActive(true);

        if(!got5thIdeya)
            ideyaLocation5.GetComponent<LineRenderer>().enabled = false; 
        else ideyaLocation5.GetComponent<LineRenderer>().enabled = true; 
    }
    
    public GameObject gPopup;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            gPopup.SetActive(true);
        
        if(other.CompareTag("levelPlayer") && other.GetComponent<LevelFollow>().continueLevel)
        {
            blueChipMat.SetFloat("_EmissionOn", 0f);
            if(freedIdeas == 1)
                ideya1.LockInPalace();
            if(freedIdeas == 2)
                ideya2.LockInPalace();
            if(freedIdeas == 3)
                ideya3.LockInPalace();
            if(freedIdeas == 4)
                ideya4.LockInPalace();
        }
    }

    public void ReturnAllIdeyas()
    {
        ideya1.ReturnToCapture();
        ideya2.ReturnToCapture();
        ideya3.ReturnToCapture();
        ideya4.ReturnToCapture();
    }
}
