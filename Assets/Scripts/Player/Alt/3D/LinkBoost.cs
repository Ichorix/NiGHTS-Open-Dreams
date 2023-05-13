using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkBoost : MonoBehaviour
{
    public PlayerBhysics Player;
    public int link;
    public bool linkActive;
    public float linkTime, linkBoostSpeed;
    public float linkTimeStart = 5;
    public float defaultTopSpeed = 75;

    void Start()
    {
        linkBoostSpeed = 50;
        linkTimeStart = 5;
        defaultTopSpeed = 50;
        Player.TopSpeed = defaultTopSpeed;
    }
    void Update()
    {
        if(linkTime > 0)
        {
            linkTime -= Time.deltaTime;
        }
        if(linkTime <= 0 && linkTime > -10)
        {
            LinkEnd();
            linkTime = -20;
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            Link();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        if(other.gameObject.CompareTag("BlueChip"))
        {
            //Sounds.PlayOneShot(BlueChipSFX, 1.0f);
            Destroy(other.gameObject);
            //chipAmount += 1;
            Link();
        }
        if(other.gameObject.CompareTag("YellowRing"))
        {
            //Sounds.PlayOneShot(YellowRingSFX, 1.0f);
            Link();
        }
        if(other.gameObject.CompareTag("GreenRing"))
        {
            //Sounds.PlayOneShot(GreenRingSFX, 1.0f);
            linkTime = linkTimeStart; 
        }
        if(other.gameObject.CompareTag("HalfRing"))
        {
            //Sounds.PlayOneShot(HalfRingSFX, 1.0f);
            Link();
        }
        if(other.gameObject.CompareTag("PowerRing"))
        {
            //Sounds.PlayOneShot(PowerRingSFX, 1.0f);
            Link();
        }
        if(other.gameObject.CompareTag("SpikeRing"))
        {
            //Sounds.PlayOneShot(SpikeRingSFX, 1.0f);
            LinkEnd();
        }
    }

    void Link()
    {
        linkTime = linkTimeStart; 
        link += 1;
        linkActive = true;
        
        Player.TopSpeed = defaultTopSpeed + link * 15;
        //Player.p_rigidbody.AddForce(transform.forward * linkBoostSpeed, ForceMode.Impulse);
        //Debug.Log("noMag" + Player.p_rigidbody.velocity + "withMag" + Player.p_rigidbody.velocity.magnitude + "sqrMag" + Player.p_rigidbody.velocity.sqrMagnitude);
    }
    void LinkEnd()
    {
        link = 0;
        linkActive = false;
        Player.TopSpeed = defaultTopSpeed;
    }
}
