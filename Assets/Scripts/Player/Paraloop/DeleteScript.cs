using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteScript : MonoBehaviour
{
    private float timeLeft = 0;
    private float timeBeforeCollidable = 0.5f;
    private float timeBeforeDelete = 2f;
    public Collider collider;
    void Update()
    {
        timeLeft += Time.deltaTime;
        if(timeLeft >= timeBeforeCollidable)
        {
            collider.enabled = true;
        }
        if(timeLeft >= timeBeforeDelete)
        {
            Destroy(gameObject);
        }
    }
}
