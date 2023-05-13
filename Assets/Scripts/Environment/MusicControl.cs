using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    public static MusicControl instance;
    public LightingManager script;

    public bool playing;
    public AudioSource musicSource;
    public AudioClip[] musicClips;
    public AudioClip DayTimeStart;
    public AudioClip DayTimeMUS;
    public AudioClip NightTimeMUS;


    public bool isPaused;
    public bool levelActive;
    //public float timeOfDay;
    public bool isDay;
    public float dayTime;

    private void Awake()
    {
        /*DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }*/
    }
    private void Start()
    {
        playing = true;
        //StartCoroutine(PlayMusicLoop());
        levelActive = false;
        isPaused = false;
    }
    void Update()
    {
        isDay = script.isDay;
        /*if(isPaused == true)
        {
            //pauseMusic
        }
        if(isPaused == false)
        {
            if(levelActive == true)
            {
                //Get Level, start level music
            }
            if(levelActive == false)
            {
                if(isDay == true)
                {
                    //Play Day Start
                    dayTime += Time.deltaTime;
                    if(dayTime >= 15)
                    {
                        musicSource.PlayOneShot(DayTimeMUS, 1.0f); 
                    }
                }
                if(isDay == false)
                {
                    musicSource.PlayOneShot(NightTimeMUS, 1.0f);
                }
            }
        }*/
    }
/*
    IEnumerator PlayMusicLoop()
    {
        yield return null;

        while(playing)
        {
            for(int i = 0; i < musicClips.Length; i++)
            {
                musicSource.clip = musicClips[i];

                musicSource.Play();
                while(musicSource.isPlaying)
                {
                    yield return null;
                }
            }
        }
    }*/
}
