using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public RingYellow yellowRing;
    public RingYellow secondRing;
    public bool TwoRings;
    public bool chip;

    public void Respawn()
    {
        if (chip)
        {
            this.gameObject.SetActive(true);
        }
        if (yellowRing != null)
        {
            yellowRing.Respawn();
            if(TwoRings)
            {
                secondRing.Respawn();
            }
        }
    }
    public void Despawn()
    {
        this.gameObject.SetActive(false);
    }
}
