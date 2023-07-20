using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePointItem : MonoBehaviour
{
    public Transform parent;
    public GameObject pointItem;
    public GameObject chipItem;

    public void InstantiatePointAndChip(bool isChip)
    {
        if(!isChip)
        {
            Instantiate(pointItem, parent);
        }
        if(isChip)
        {
            Instantiate(chipItem, parent);
        }
    }
}
