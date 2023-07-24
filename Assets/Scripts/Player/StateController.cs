using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public GameObject currentPlayer;
    public GameObject playerPrefab, playerPrefabPlayer;//1
    public GameObject playerGrounded, playerGroundedPlayer;//2
    public GameObject playerStage, playerStagePlayer;//3
    
    public bool state1, state2, state3, hasJumped;
    public float hasJumpedTime;

    public GameObject boostBarUI, chipsUI, scoreUI, timeUI;

    void Start()
    {
        Activate1();
    }
    void Update()
    {
        if(hasJumped == true)
        {
            hasJumpedTime += Time.deltaTime;
        }
        else{hasJumpedTime = 0;}
        if(hasJumpedTime >= 1f)
        {
            Activate1();
            hasJumped = false;
        }

        
    }

    public void Activate1()
    {
        //switchCoolDown = 0f;
        if(state2 == true)
        {
            Debug.Log("Was state 2");
            playerPrefabPlayer.transform.position = playerGroundedPlayer.transform.position;
            playerPrefabPlayer.transform.rotation = playerGroundedPlayer.transform.rotation;
        }
        if(state3 == true)
        {
            Debug.Log("Was state 3");
            playerPrefabPlayer.transform.position = playerStagePlayer.transform.position;
        }
        playerPrefab.SetActive(true);
        playerGrounded.SetActive(false);
        playerStage.SetActive(false);

        currentPlayer = playerPrefabPlayer;
        playerPrefabPlayer.transform.Translate(Vector3.up * 1.7f);

        state1 = true;
        state2 = false;
        state3 = false;

        boostBarUI.SetActive(true);
        chipsUI.SetActive(true);
        scoreUI.SetActive(false);
        timeUI.SetActive(false);
    }
    public void Activate2()
    {
        //switchCoolDown = 0f;
        playerPrefab.SetActive(false);
        playerGrounded.SetActive(true);
        playerStage.SetActive(false);

        state1 = false;
        state2 = true;
        state3 = false;

        playerGroundedPlayer.transform.position = playerPrefabPlayer.transform.position;
        playerGroundedPlayer.transform.rotation = playerPrefabPlayer.transform.rotation;

        currentPlayer = playerGroundedPlayer;
        playerGroundedPlayer.transform.Translate(Vector3.up * 1.7f);

        boostBarUI.SetActive(false);
        chipsUI.SetActive(true);
        scoreUI.SetActive(false);
        timeUI.SetActive(false);
    }
    public void Activate3(int levelNumber)
    {
        //switchCoolDown = 0f;
        playerPrefab.SetActive(false);
        playerGrounded.SetActive(false);
        playerStage.SetActive(true);

        state1 = false;
        state2 = false;
        state3 = true;

        currentPlayer = playerStagePlayer;

        boostBarUI.SetActive(true);
        chipsUI.SetActive(true);
        scoreUI.SetActive(true);
        timeUI.SetActive(true);
    }
    public void HasJumped()
    {
        hasJumped = true;
    }
}
