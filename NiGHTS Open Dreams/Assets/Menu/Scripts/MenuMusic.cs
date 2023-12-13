using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    public MenuNights menuNights;
    public AudioSource Screens;
    public AudioSource Opening;
    public AudioClip OpeningC;
    public AudioSource Violin;
    public AudioSource Main;
    public AudioSource Options;
    
    public bool playedScreens, playedOpening, playedViolin;
    
    public float MusicVolume = 1.0f;    
    void Start()
    {
        //Opening.PlayDelayed(24.316f);
        //Violin.PlayDelayed(36.906f);
    }

    void Update()
    {
        if(!Screens.isPlaying && !playedScreens)
        {
            Opening.Play();
            playedScreens = true;
            menuNights.StartFall();
        }
        if(!Opening.isPlaying && playedScreens && !playedOpening)
        {
            Violin.Play();
            playedOpening = true;
        }
    }
}
