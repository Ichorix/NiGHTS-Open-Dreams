using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

public class IdeyaPickup : MonoBehaviour
{
    public int chipReq;
    public GameObject thisPlayer;
    public LevelFollow Lplayer;
    public IdeyaFollow ideya;
    public int ideyaNum;
    public Material blueChipMat;
    public GrowthPalace palace;

    void Start()
    {
        Lplayer = thisPlayer.GetComponent<LevelFollow>();
        ideya.player = thisPlayer;
        ideya.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("levelPlayer") && Lplayer.chipCounter >= chipReq)
        {
            blueChipMat.SetFloat("_EmissionOn", 1f);
            Lplayer.continueLevel = true;
            ideya.enabled = true;
            palace.freedIdeas = ideyaNum;

            palace.UpdateStuff();
        }
    }
}
