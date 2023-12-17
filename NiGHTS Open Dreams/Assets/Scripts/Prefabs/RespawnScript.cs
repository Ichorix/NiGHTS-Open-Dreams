using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public RingYellow yellowRing;
    public RingYellow secondRing;
    public bool LOD;
    public bool TwoRings;
    public bool chip;

    public void Respawn()
    {
        if (chip)
        {
            this.gameObject.SetActive(true);
            return;
        }
        
        if (yellowRing != null)
        {
            yellowRing.Respawn();
            if (TwoRings)
            {
                secondRing.Respawn();
            }
            if (LOD)
            {
                this.gameObject.SetActive(true);
            }
        }
    }
    public void Despawn()
    {
        this.gameObject.SetActive(false);
    }
}
