using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOverTime : MonoBehaviour
{
    [SerializeField] private float timeToDelete = 1.5f;
    private bool markedForDelete = false;
    IEnumerator Start()
    {
        markedForDelete = true;
        yield return new WaitForSeconds(timeToDelete);
        Destroy(gameObject);
    }
    void OnEnable()
    {
        // Using this method because if the gameObject was disabled when the WaitForSeconds() is up, then it would not actually destroy the object
        if(markedForDelete)
            Destroy(gameObject);
    }
}
