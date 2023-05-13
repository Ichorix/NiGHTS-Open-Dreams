using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdeyaGlowScript : MonoBehaviour
{
    public Material glowMat;

    public float speedFade;
    public float sinCount;
    private float count;

    public Color red, green, blue, yellow, white;

    void Start()
    {
        glowMat.SetFloat("_Alpha", 1);
        NextColor();
    }

    void Update()
    { 
        count += speedFade * Time.deltaTime;

        sinCount = Mathf.Sin(count);
        if(sinCount <= 0)
        {
            sinCount *= -1f;
        }
        if(sinCount <= 0.05f)
        {
            NextColor();
        }
    
        glowMat.SetFloat("_Alpha", sinCount);
    }

    void NextColor()
    {
        int getColor = Random.Range(0,5);
        Color currentColor;

        float getSpeedX = Random.Range(-20,20);
        getSpeedX *= 0.1f;
        float getSpeedY = Random.Range(-20,20);
        getSpeedY *= 0.1f;
        
        if(getColor == 0)//red
        {
            currentColor = red;
            glowMat.SetColor("_Color", currentColor);
        }
        if(getColor == 1)//green
        {
            currentColor = green;
            glowMat.SetColor("_Color", currentColor);
        }
        if(getColor == 2)//blue
        {
            currentColor = blue;
            glowMat.SetColor("_Color", currentColor);
        }
        if(getColor == 3)//yellow
        {
            currentColor = yellow;
            glowMat.SetColor("_Color", currentColor);
        }
        if(getColor == 4)//white
        {
            currentColor = white;
            glowMat.SetColor("_Color", currentColor);
        }

        glowMat.SetFloat("_SpeedX", getSpeedX);
        glowMat.SetFloat("_SpeedY", getSpeedY);
    }

}
