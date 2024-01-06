using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Disclaimer : MonoBehaviour
{
    public SceneFade fade;
    public void Next()
    {
        Debug.Log("Next");
        SceneManager.LoadScene("SAGE23");
    }

    public void Continue(InputAction.CallbackContext context)
    {
        Debug.Log("Continue");
        fade.BeginFade(1);

        Next();
    }
}
