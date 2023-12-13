using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColliders : MonoBehaviour
{
    public EnemyMain main;
    public Collider thisCol;

    public bool isAggroRange;
    public bool isAttackingRange;
    public bool isFleeRange;
    public bool isDamageBox;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("levelPlayer"))
        {
            if(isAggroRange)
            {
                Debug.Log("MoveTowardsPlayer");
                main.moveTowardsPlayer = true;
            }
            if(isAttackingRange)
            {
                Debug.Log("Attacking");
                main.moveTowardsPlayer = false;
                if(main.hollowAttackScript != null) main.hollowAttackScript.StartAttack();
                else Debug.Log("No Attacks Found");
            }
            if(isFleeRange)
            {
                Debug.Log("Fleeee");
                if(main.hollowAttackScript != null) main.hollowAttackScript.StopAttack();
                main.fleeFromPlayer = true;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("levelPlayer"))
        {
            if(isAggroRange)
            {
                main.moveTowardsPlayer = false;
            }
            if(isAttackingRange)
            {
                main.moveTowardsPlayer = true;
                if(main.hollowAttackScript != null) main.hollowAttackScript.StopAttack();
                else Debug.Log("No Attacks Found");
            }
            if(isFleeRange)
            {
                main.fleeFromPlayer = false;
                if(main.hollowAttackScript != null) main.hollowAttackScript.StartAttack();
            }
        }
    }
}
