using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingYellow : MonoBehaviour
{
    public bool isCollected;
    [SerializeField] private float timeUntilGone;
    public float TimeUntilGone
    {
        get{ return timeUntilGone; }
        set
        {
            timeUntilGone = Mathf.Clamp01(value);
            if(timeUntilGone <= 0)
            {
                this.gameObject.SetActive(false);
                if (LOD) this.transform.parent.gameObject.SetActive(false);
            }
        }
    }
    public Animator YellowRingAnim;
    public string currentTag;
    public bool LOD;

    void Start()
    {
        isCollected = false;
        TimeUntilGone = 1f;
        currentTag = this.tag;
    }

    void Update()
    {
        if(isCollected)
        {
            TimeUntilGone -= Time.deltaTime;
            this.tag = "Collected";
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")||other.CompareTag("levelPlayer"))
        {
            Collect();
        }
    }
    public void Respawn()
    {
        isCollected = false;
        TimeUntilGone = 1f;
        this.tag = currentTag;
        this.gameObject.SetActive(true);
        YellowRingAnim.SetTrigger("TrRespawn");
    }

    public void Collect()
    {
        isCollected = true;
        YellowRingAnim.SetTrigger("TrCollected");
    }
}
