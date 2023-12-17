using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteScript : MonoBehaviour
{
    private float timeLeft = 0;
    private float timeBeforeCollidable = 0.5f;
    private float timeBeforeDelete = 2f;
    public Collider collider;
    [SerializeField] private bool stillInsideSelf;
    void Update()
    {
        timeLeft += Time.deltaTime;
        if(timeLeft >= timeBeforeCollidable && !stillInsideSelf)
        {
            collider.enabled = true;
        }
        if(timeLeft >= timeBeforeDelete)
        {
            Destroy(gameObject);
        }
    }

    // Manually setting if the trail is still inside the trigger detector to prevent paralooping at slow speeds
    /* TODO  // But it actually doesnt work because the collider doesnt turn on till later
        // So you will always be able to go even slower to have it count
    void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<TriggerTestTwo>() != null)
        {
            stillInsideSelf = true;
            return;
        }
        if(other.CompareTag("Player"))
        {
            stillInsideSelf = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<TriggerTestTwo>() != null)
        {
            stillInsideSelf = false;
            Debug.Log("Left Trigger");
            return;
        }
        if(other.CompareTag("Player"))
        {
            stillInsideSelf = false;
            Debug.Log("Left Player");
        }
    }
    */
}
