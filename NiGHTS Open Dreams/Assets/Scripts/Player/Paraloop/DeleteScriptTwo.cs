using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteScriptTwo : MonoBehaviour
{
    public float timeLeft = 0;
    public float timeToDelete = 1.5f;
    void Update()
    {
        timeLeft += Time.deltaTime;
        if(timeLeft >= timeToDelete)
        {
            Destroy(gameObject);
        }
    }
}
