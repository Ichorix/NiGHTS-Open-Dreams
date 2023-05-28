using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

public class EnemyMain : MonoBehaviour
{
    public float moveSpeed;
    public bool moveTowardsPlayer;
    public bool fleeFromPlayer;

    public StateController playerStates;
    public GameObject currentPlayer;

    public HollowAttack hollowAttackScript;

    void OnEnable()
    {///Reused from Paraloop Collect Script
    /*
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
    */
    }

    void Update()
    {
        if(moveTowardsPlayer && !fleeFromPlayer)
        {
            transform.LookAt(currentPlayer.transform);
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
        if(fleeFromPlayer)
        {
            transform.LookAt(currentPlayer.transform);
            Quaternion rotation = Quaternion.LookRotation(-transform.forward, Vector3.up);
            transform.rotation = rotation;
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
    }
}
