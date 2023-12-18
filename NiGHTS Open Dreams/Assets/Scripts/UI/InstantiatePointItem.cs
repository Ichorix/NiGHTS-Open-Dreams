using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstantiatePointItem : MonoBehaviour
{
    public Transform parent;
    public GameObject pointItem;
    public GameObject chipItem;
    public GameObject damageItem;

    public Material PointItemMaterial;
    [ColorUsage(true, true)]
    public Color link1;
    [ColorUsage(true, true)]
    public Color link6;
    [ColorUsage(true, true)]
    public Color link10;

    public void InstantiateUItem(int item, float value = 0)
    {
        switch(item)
        {
            case 1: // Chip
                Instantiate(chipItem, parent);
                break;
            case 2: // Points
                GameObject pointItemRef = Instantiate(pointItem, parent);
                pointItemRef.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = value.ToString();
                Debug.Log("1");
                if(PointItemMaterial != null)
                {
                    PointNChipCircles pointItemScript = pointItemRef.GetComponent<PointNChipCircles>();
                    pointItemScript.matInstance = Instantiate(PointItemMaterial);
                    
                    if(value >= 100)
                        pointItemScript.matInstance.SetColor("_FullColor", link10);
                    else if(value > 50)
                        pointItemScript.matInstance.SetColor("_FullColor", link6);
                    else
                        pointItemScript.matInstance.SetColor("_FullColor", link1);
                    
                    pointItemScript.image = pointItemRef.GetComponent<Image>();
                    pointItemScript.image.material = pointItemScript.matInstance;
                    Debug.Log("7");
                }
                Debug.Log("Got to here just fine");
                break;
            case 3: // Damage
                GameObject damageItemRef = Instantiate(damageItem, parent);
                damageItemRef.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = value.ToString();
                break;
        }

    }
}
