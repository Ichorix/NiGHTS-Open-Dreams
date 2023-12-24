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
    private Collider selfCollider;
    public bool LOD;
    public bool dontDisableCollider;

    void Awake()
    {
        selfCollider = GetComponent<Collider>();
    }
    void Start()
    {
        isCollected = false;
        TimeUntilGone = 1f;
        selfCollider.enabled = true;
    }

    void Update()
    {
        if(isCollected)
        {
            TimeUntilGone -= Time.deltaTime;
            if(!dontDisableCollider)
                selfCollider.enabled = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Collect();
        }
    }
    public void Respawn()
    {
        isCollected = false;
        TimeUntilGone = 1f;
        selfCollider.enabled = true;
        this.gameObject.SetActive(true);
        YellowRingAnim.SetTrigger("TrRespawn");
    }

    public void Collect()
    {
        isCollected = true;
        YellowRingAnim.SetTrigger("TrCollected");
    }
}
