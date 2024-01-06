using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowSoundsThroughPause : MonoBehaviour
{
    public AudioSource[] audioSources;
    void Start()
    {
        foreach(AudioSource source in audioSources)
        {
            source.ignoreListenerPause = true;
        }
    }
}
