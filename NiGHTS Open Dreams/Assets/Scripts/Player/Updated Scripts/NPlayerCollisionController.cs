using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPlayerCollisionController : MonoBehaviour
{
    [SerializeField] private NPlayerLevelFollow levelPlayer;
    [SerializeField] private NPlayerOpenControl openPlayer;
    [SerializeField] private InstantiatePointItem pointItemScript;
    [SerializeField] private NPlayerScriptableObject _stats;
    [SerializeField] private SoundPlayerScriptableObject _sounds;
    public AudioSource MainSounds;

    void OnTriggerEnter(Collider other)
    {
        // Bump in case you actually triggered with the ground
        if(other.CompareTag("lb_groundTarget"))
        {
            if(openPlayer != null)
            {
                //Sets the player back by a frame along with bumping them up. Makes it more consistent
                openPlayer.BumpUpFromGround(100, openPlayer._speed * Time.deltaTime);
            }
            else if(levelPlayer != null)
            {
                levelPlayer.GetComponent<Rigidbody>().AddForce(Vector3.up * 50, ForceMode.Impulse);
            }
            return;
        }
        
        CollectablesData item = other.GetComponent<Collectable>()?.data;
        if(item != null)
            CollectItem(other, item);
    }
    // Constant Ground Orientation
    void OnCollisionStay(Collision other)
    {
        if(other.gameObject.CompareTag("lb_groundTarget"))
        {
            if(openPlayer != null)
            {
                openPlayer.ReAdjustToNormals(other.contacts[0].normal);
                openPlayer._animations.Grounded = true;
            }
            else if(levelPlayer != null)
            {
                levelPlayer.GetComponent<Rigidbody>().AddForce(Vector3.up * 300);
            }
        }
    }
    // Little Hop after leaving the ground
    void OnCollisionExit(Collision other)
    {
        if(other.gameObject.CompareTag("lb_groundTarget") && openPlayer != null)
        {
            openPlayer._animations.Grounded = false;
            openPlayer.BumpUpFromGround(30);
        }
    }

    public void CollectItem(Collider other, CollectablesData data)
    {
        if(levelPlayer != null)
        {
            int points = data.Score * (data.timesLink ? levelPlayer.link : 1);

            if(data.increaseLink) levelPlayer.LinkIncrease();
            else if(data.clearLink) levelPlayer.LinkEmpty();

            levelPlayer.currentScore += points;
            if(points > 0)
                pointItemScript.InstantiateUItem(2, points);

            levelPlayer.LevelTimeLeft += data.Time;
            if(data.Time < 0)
                pointItemScript.InstantiateUItem(3, data.Time);
            levelPlayer.currentChips += data.Chips;
        }
        else if(openPlayer != null)
        {
            _stats.openChips += data.Chips;
        }
        if(data.Chips > 0)
            pointItemScript.InstantiateUItem(1);

        _stats.PowerBuff = data.givePower;
        _stats.PowerBuffTimeLeft = data.powerTime;
        _stats.BoostGauge += data.Boost;

        if(data.instantOff) other.gameObject.SetActive(false);
        MainSounds.PlayOneShot(data.interactionSound, 1.0f);
    }
}
