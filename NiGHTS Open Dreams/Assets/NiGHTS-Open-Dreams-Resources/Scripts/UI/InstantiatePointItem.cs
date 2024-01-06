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
    public GameObject scoreSpinner;

    public Material PointItemMaterial;
    [ColorUsage(true, true)]
    public Color link1;
    public Sprite border1;
    [ColorUsage(true, true)]
    public Color link6;
    public Sprite border6;
    [ColorUsage(true, true)]
    public Color link10;
    public Sprite border10;

    public void InstantiateUItem(int item, float value = 0)
    {
        switch(item)
        {
            case 1: // Chip
                Instantiate(chipItem, parent);
                break;
            case 2: // Points
                GameObject pointItemRef = Instantiate(pointItem, parent);
                pointItemRef.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = value.ToString();
                if(PointItemMaterial != null)
                {
                    PointNChipCircles pointItemScript = pointItemRef.GetComponent<PointNChipCircles>();
                    pointItemScript.matInstance = Instantiate(PointItemMaterial);
                    pointItemScript.image = pointItemRef.GetComponent<Image>();

                    if(value >= 100)
                    {
                        pointItemScript.borderVariant = border10;
                        pointItemScript.matInstance.SetColor("_FullColor", link10);
                    }
                    else if(value > 50)
                    {
                        pointItemScript.borderVariant = border6;
                        pointItemScript.matInstance.SetColor("_FullColor", link6);
                    }
                    else
                    {
                        pointItemScript.borderVariant = border1;
                        pointItemScript.matInstance.SetColor("_FullColor", link1);
                    }
                    
                    pointItemScript.image.material = pointItemScript.matInstance;
                }
                break;
            case 3: // Damage
                GameObject damageItemRef = Instantiate(damageItem, parent);
                damageItemRef.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = value.ToString();
                break;
            case 4: // Score Spinner
                Instantiate(scoreSpinner, parent);
                break;
        }

    }
}
