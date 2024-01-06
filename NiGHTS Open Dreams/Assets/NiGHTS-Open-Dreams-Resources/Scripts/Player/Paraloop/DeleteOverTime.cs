using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOverTime : MonoBehaviour
{
    [SerializeField] private float timeToDelete = 1.5f;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(timeToDelete);
        Destroy(gameObject);
    }
}
