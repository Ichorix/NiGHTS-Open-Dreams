using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SAGESPLASH : MonoBehaviour
{
    public AudioSource aSource;

    void Start()
    {
        Invoke("AudioFinish", aSource.clip.length);
    }
    void AudioFinish()
    {
        Debug.Log("Change Scenes");
        SceneManager.LoadScene("MenusBasic");
    }
}
