using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class PressTextAlpha : MonoBehaviour {
 
    public float speedFade;
    private float count;
    public float brightness;
    public Image image;
    public float sinCount;
    
    void Start()
    {
        speedFade = 1;
        brightness = 0.7f;
    }
    void Update()
    { 
    //Fade in-out press start
    count += speedFade * Time.deltaTime;

    sinCount = Mathf.Sin(count);
    if(sinCount <= 0)
    {
        sinCount *= -1f;
    }
  
    image.color = new Color(1f, 1f, 1f, sinCount * brightness);
 
    }
}
