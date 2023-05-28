using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HollowAttack : MonoBehaviour
{
    public EnemyMain main;
    public GameObject hollowProjectile;
    public float timeBetweenAttack;
    IEnumerator HollowAttacking()
    {
        while(true)
        {
            transform.LookAt(main.currentPlayer.transform);
            Instantiate(hollowProjectile, transform.position, transform.rotation);
            yield return new WaitForSeconds(timeBetweenAttack);
        }
    }

    public void StartAttack()
    {
        StartCoroutine(HollowAttacking());
    }
    public void StopAttack()
    {
        StopAllCoroutines();
    }
}
