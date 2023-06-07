using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingYellow : MonoBehaviour
{
    public bool isCollected;
    private float timeUntilGone;
    public Animator YellowRingAnim;
    public string currentTag;
    public bool LOD;

    void Start()
    {
        isCollected = false;
        timeUntilGone = 1f;
        currentTag = this.tag;
    }

    void Update()
    {
        if(isCollected)
        {
            timeUntilGone -= Time.deltaTime;
            Debug.Log("Is Collected");
            this.tag = "Collected";
        }
        if(timeUntilGone <= 0)
        {
            this.gameObject.SetActive(false);
            if (LOD) this.transform.parent.gameObject.SetActive(false);
            Debug.Log("Done COllected");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")||other.CompareTag("levelPlayer"))
        {
            isCollected = true;
            YellowRingAnim.SetTrigger("TrCollected");
        }
    }
    public void Respawn()
    {
        Debug.Log("Respawn2");
        isCollected = false;
        timeUntilGone = 1f;
        this.tag = currentTag;
        this.gameObject.SetActive(true);
        YellowRingAnim.SetTrigger("TrRespawn");
        Debug.Log("REspawn3");
    }
}
