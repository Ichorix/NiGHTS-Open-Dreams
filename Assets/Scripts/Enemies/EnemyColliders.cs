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

    void OnTriggerEnter()
    {

    }


}
