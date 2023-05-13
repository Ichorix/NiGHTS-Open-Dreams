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
            if(other.tag == "Player")
            {
                //DO the grab thing
            }
            if(other.tag == "Player" || other.tag == "Paraloop")//Replace player with the drilldash effect
            {
                main.Damage();
            }
        }
        if(!main.isVulnerable && other.tag == "Player" && isDamageBox)
        {
            Debug.Log("Damage Player");
            player.takeDamage(Vector3.zero, 0);
        }
        if(!main.isVulnerable && other.tag == "Paraloop" && isDamageBox)
        {
            main.Damage();
        }

        if(isSawRange && other.tag == "Player" && main.sawWhenNear)
        {
            //main.playerIsNear = true;
            Debug.Log("Sawwww");
            main.SawIt(false);
        }

        if(isLeftZone && other.tag == "Player")
        {
            main.playerIsLeft = true;
        }
        if(isRightZone && other.tag == "Player")
        {
            main.playerIsRight = true;
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if(isLeftZone && other.tag == "Player")
        {
            //Debug.Log("Player on Left");
            main.playerIsLeft = true;
        }
        if(isRightZone && other.tag == "Player")
        {
            //Debug.Log("Player on Right");
            main.playerIsRight = true;
        }
        /// Add some stuff for left and right zone if necessary
    }
    public void OnTriggerExit(Collider other)
    {
        if(isSawRange && other.tag == "Player" && main.sawWhenNear)
        {
            //main.playerIsNear = true;
            Debug.Log("Un-Sawwww");
            main.leftHandScript.Return();
            main.rightHandScript.Return();
            main.isSawing = false;
            
            if(main._Phase == 13) main.Phase13();
            if(main._Phase == 12) main.Phase12();
            if(main._Phase == 11) main.Phase11();
        }

        if(isLeftZone && other.tag == "Player")
        {
            main.playerIsLeft = false;
        }
        if(isRightZone && other.tag == "Player")
        {
            main.playerIsRight = false;
        }
    }
}
