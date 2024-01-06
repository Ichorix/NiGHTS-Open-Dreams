using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LinkControl : MonoBehaviour
{
    public TextMeshProUGUI linkText;
    public AnimationCurve curve;
    [SerializeField] private float speed;
    private float scaleTime;
    private float scale;

    void OnEnable()
    {
        RunLinkIncrease(0);
    }

    public void RunLinkIncrease(int link)
    {
        if(link > 1)
        {
            linkText.text = link.ToString() + "Links";
            scaleTime = 0;
        }
        if(link == 0) scaleTime = 1;
    }
    
    void Update()
    {
        if(scaleTime <= 1)
        {
            scaleTime += Time.deltaTime * speed;
            scale = curve.Evaluate(scaleTime);
            
            transform.localScale = new Vector3(
                scale, scale, scale
            );
        }
    }
}
