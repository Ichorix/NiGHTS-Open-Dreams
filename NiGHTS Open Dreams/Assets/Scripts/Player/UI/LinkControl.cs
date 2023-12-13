using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LinkControl : MonoBehaviour
{
    public TextMeshProUGUI linkText;
    public int link;
    public AnimationCurve curve;
    [SerializeField] private float speed;
    public float time;
    public float scale;

    void OnEnable()
    {
        RunLinkIncrease();
    }
    public void RunLinkIncrease()
    {
        if(link > 1)
        {
            linkText.text = link.ToString() + "Links";
            time = 0;
        }
        if(link == 0) time = 1;
    }
    
    void Update()
    {
        if(time <= 1)
        {
            time += Time.deltaTime * speed;
            scale = curve.Evaluate(time);
            
            transform.localScale = new Vector3(
                scale, scale, scale
            );
        }
    }
}
