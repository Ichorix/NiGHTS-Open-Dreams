using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;

public class SAGESPLASH : MonoBehaviour
{
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TextMeshProUGUI percentageText;
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
        AsyncOperation operation = SceneManager.LoadSceneAsync("OpenDreams");
        while(!operation.isDone)
        {
            float progress = operation.progress; // 0 - 1
            percentageText.text = progress.ToString("p0"); // Formats as percentage with no decimals
            loadingBar.value = Mathf.Clamp(progress * 16, 1, 16);
            yield return null;
        }
    }
    
}
