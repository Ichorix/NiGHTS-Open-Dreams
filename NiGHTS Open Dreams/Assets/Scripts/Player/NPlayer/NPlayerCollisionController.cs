using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPlayerCollisionController : MonoBehaviour
{
    [SerializeField] private NPlayerLevelFollow levelFollow;
    [SerializeField] private NPlayerOpenControl openControl;
    [SerializeField] private InstantiatePointItem pointItemScript;
    [SerializeField] private NPlayerScriptableObject _stats;
    public AudioSource MainSounds;

    void OnTriggerEnter(Collider other)
    {
        // Bump in case you actually triggered with the ground
        if(other.CompareTag("lb_groundTarget"))
        {
            if(openControl != null)
            {
                //Sets the player back by a frame along with bumping them up. Makes it more consistent
                openControl.BumpUpFromGround(100, openControl._speed * Time.deltaTime);
            }
            else if(levelFollow != null)
            {
                levelFollow.GetComponent<Rigidbody>().AddForce(Vector3.up * 50, ForceMode.Impulse);
            }
            return;
        }
        
        CollectablesData item = other.GetComponent<Collectable>()?.data;
        if(item != null)// Amazing code
            if(item.dashBall)
            {
                if(!levelFollow.specialBehaviourActive)
                StartCoroutine(levelFollow.Behaviour_DashBall(item.launchTime, item.launchSpeed));
            }
            else
                CollectItem(other, item);
    }

    // Constant Ground Orientation
    void OnCollisionStay(Collision other)
    {
        if(other.gameObject.CompareTag("lb_groundTarget"))
        {
            if(openControl != null)
            {
                openControl.ReAdjustToNormals(other.contacts[0].normal);
                openControl._animations.Grounded = true;
            }
            else if(levelFollow != null)
            {
                levelFollow.GetComponent<Rigidbody>().AddForce(Vector3.up * 300);
            }
        }
    }

    // Little Hop after leaving the ground
    void OnCollisionExit(Collision other)
    {
        if(other.gameObject.CompareTag("lb_groundTarget") && openControl != null)
        {
            openControl._animations.Grounded = false;
            openControl.BumpUpFromGround(30);
        }
    }

    public void CollectItem(Collider other, CollectablesData data)
    {
        int points = 0;
        if(levelFollow != null)
        {
            if(data.increaseLink) levelFollow.LinkIncrease();
            else if(data.clearLink) levelFollow.LinkEmpty();

            points = data.Score * (data.timesLink ? levelFollow.link : 1);

            levelFollow.currentScore += points;
            if(points > 0)
                pointItemScript.InstantiateUItem(2, points);

            levelFollow.LevelTimeLeft += data.Time;
            if(data.Time < 0)
                pointItemScript.InstantiateUItem(3, data.Time);
            levelFollow.currentChips += data.Chips;
        }
        else if(openControl != null)
        {
            openControl.OpenChips += data.Chips;

            if(data.increaseLink) openControl.LinkIncrease();
            else if(data.clearLink) openControl.LinkEmpty();
            
            points = data.Score * (data.timesLink ? openControl.link : 1);
            if(points > 0)
                pointItemScript.InstantiateUItem(2, points);
        }
        if(data.Chips > 0)
            pointItemScript.InstantiateUItem(1);

        if(data.givePower)
        {
            _stats.PowerBuffTimeLeft = data.powerTime > _stats.PowerBuffTimeLeft ? data.powerTime : _stats.PowerBuffTimeLeft;
            _stats.PowerBuff = true;
        }


        _stats.BoostGauge += data.Boost;

        if(data.instantOff) other.gameObject.SetActive(false);
        MainSounds.PlayOneShot(data.interactionSound, 1.0f);
    }
}
