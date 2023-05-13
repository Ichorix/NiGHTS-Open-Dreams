using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

public class CollectItems : MonoBehaviour
{
    public StateController playerStates;
    public GameObject currentPlayer;

    public NewPlayerControl openPlayer;
    public LevelFollow levelPlayer;

    public bool isBlueChip;
    public bool isStarChip;

    void Start()
    {
        //////////NASTY CODE DONT LOOK AT ME//////////
        //Determine Player
        playerStates = GameObject.Find("NiGHTSPlayerStates").GetComponent<StateController>();
        currentPlayer = playerStates.currentPlayer;

        if(currentPlayer.ToString() == "Player (UnityEngine.GameObject)")// Open
        {
            openPlayer = currentPlayer.GetComponent<NewPlayerControl>();
        }
        if(currentPlayer.ToString() == "NiGHTS_Updated (UnityEngine.GameObject)")// Level
        {
            levelPlayer = currentPlayer.GetComponent<LevelFollow>();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Paraloop"))
        {
            if(isBlueChip)
            {
                if(openPlayer != null)
                {
                    openPlayer.Sounds.PlayOneShot(openPlayer.BlueChipSFX, 1.0f);
                    Destroy(this.gameObject);
                    openPlayer.chipAmount += 1;
                }
                if(levelPlayer != null)
                {
                    levelPlayer.Sounds.pitch = 1;
                    levelPlayer.Sounds.PlayOneShot(levelPlayer.BlueChipSFX, 1.0f);
                    Destroy(this.gameObject);
                    levelPlayer.score += 20;
                    levelPlayer.chipCounter += 1;

                    levelPlayer.LinkIncrease();
                }
            }
            if(isStarChip)
            {
                levelPlayer.Sounds.pitch = 1;
                levelPlayer.Sounds.PlayOneShot(levelPlayer.BlueChipSFX, 1.0f);
                Destroy(this.gameObject);
                levelPlayer.score += 20;
                levelPlayer.levelTimeLeft++;
                levelPlayer.LinkIncrease();
            }
        }
    }


    /*IEnumerator GotParalooped()
    {
        //While not dead;
        //lerp: paraloop.transform.position;
        //when t=1; destroy(other)
    }*/
}
