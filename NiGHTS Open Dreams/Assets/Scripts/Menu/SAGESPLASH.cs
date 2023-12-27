using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SAGESPLASH : MonoBehaviour
{
    [SerializeField] private Slider loadingBar;
    [SerializeField] private VideoPlayer videoPlayer;

    void Start()
    {
        loadingBar.gameObject.SetActive(false);
        videoPlayer.loopPointReached += EndReached;
    }

    void EndReached(VideoPlayer vp)
    {
        loadingBar.gameObject.SetActive(true);
        StartCoroutine(LoadingProgress());
    }

    private IEnumerator LoadingProgress()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("TerrainMaker");
        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress * 16);
            loadingBar.value = progress;
            yield return null;
        }
    }
    
}
