using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

public class IdeyaPickup : MonoBehaviour
{
    public int chipReq;
    public bool collected;
    public GameObject thisPlayer;
    public LevelFollow Lplayer;
    public IdeyaFollow ideya;
    public Material blueChipMat;

    void Start()
    {
        Lplayer = thisPlayer.GetComponent<LevelFollow>();
        ideya.player = thisPlayer;
        ideya.enabled = false;
        collected = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("levelPlayer") && Lplayer.chipCounter >= chipReq)
        {
            blueChipMat.SetFloat("_EmissionOn", 1f);
            Lplayer.continueLevel = true;
            ideya.enabled = true;
            collected = true;
        }
    }

    void OnDisable()
    {//glowMat.SetFloat("_Alpha", sinCount);
        blueChipMat.SetFloat("_EmissionOn", 0f);
        collected = false;
    }
}
