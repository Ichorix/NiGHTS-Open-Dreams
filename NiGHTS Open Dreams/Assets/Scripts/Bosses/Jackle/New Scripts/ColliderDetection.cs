using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

public class ColliderDetection : MonoBehaviour
{
    public Collider collider;
    public Jackle main;
    public LevelFollow player;
    public bool isDamageBox, isSawRange, isLeftZone, isRightZone;
    public bool shouldTakeDamage, shouldDamagePlayer, shouldEnterSaw, isLeft, isRight;



    public void OnTriggerEnter(Collider other)
    {
        if(main.isVulnerable && isDamageBox)
        {
            if(other.CompareTag("levelPlayer"))
            {
                //DO the grab thing
            }
            if(other.CompareTag("Paraloop"))//Replace player with the drilldash effect
            {
                main.Damage();
            }
        }
        if(!main.isVulnerable && other.CompareTag("levelPlayer") && isDamageBox)
        {
            Debug.Log("Damage Player");
            player.takeDamage();
        }
        if(!main.isVulnerable && other.CompareTag("Paraloop") && isDamageBox)
            main.Damage();

        if(isSawRange && other.CompareTag("levelPlayer"))
            main.playerIsNear = true;

        if(isLeftZone && other.CompareTag("levelPlayer"))
            main.playerIsLeft = true;
        if(isRightZone && other.CompareTag("levelPlayer"))
            main.playerIsRight = true;
    }
    public void OnTriggerStay(Collider other)
    {
        if(isLeftZone && other.CompareTag("levelPlayer"))
            main.playerIsLeft = true;
        if(isRightZone && other.CompareTag("levelPlayer"))
            main.playerIsRight = true;
    }
    public void OnTriggerExit(Collider other)
    {
        if(isSawRange && other.CompareTag("levelPlayer"))
            main.playerIsNear = false;

        if(isLeftZone && other.CompareTag("levelPlayer"))
            main.playerIsLeft = false;
        if(isRightZone && other.CompareTag("levelPlayer"))
            main.playerIsRight = false;
    }
}
