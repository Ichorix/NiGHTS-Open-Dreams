using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoFade : MonoBehaviour
{
    public RawImage image;
    public VideoPlayer videoPlayer;
    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000;
    public float alpha = 1;
    public int fadeDir = 1;

    void Start()
    {
        videoPlayer.loopPointReached += VideoFinish;
    }
    void VideoFinish(VideoPlayer vp)
    {
        Debug.Log("Finished");
        FadeTexture(-1);
    }
    void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;

        alpha = Mathf.Clamp01(alpha);

        //GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        //GUI.depth = drawDepth;
        //GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), image);

        image.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
    }

    public float FadeTexture(int direction)
    {
        fadeDir = direction;
        return(fadeSpeed);
    }

    void OnLevelWasLoaded()
    {
        FadeTexture(1);
    }
}
