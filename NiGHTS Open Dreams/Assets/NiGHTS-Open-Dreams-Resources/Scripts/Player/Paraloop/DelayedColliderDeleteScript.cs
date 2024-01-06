using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedColliderDeleteScript : MonoBehaviour
{
    private float timeBeforeCollidable = 0.5f;
    private float timeBeforeDelete = 2f;
    public Collider collider;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(timeBeforeCollidable);
        collider.enabled = true;
        yield return new WaitForSeconds(timeBeforeDelete - timeBeforeCollidable);
        Destroy(gameObject);
    }
}
