using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

public class IdeyaPickup : MonoBehaviour
{
    public int chipReq;
    public LevelFollow player;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("levelPlayer"))
        {
            if (player.chipCounter >= chipReq)
            {
                player.continueLevel = true;
            }
        }
    }
}
