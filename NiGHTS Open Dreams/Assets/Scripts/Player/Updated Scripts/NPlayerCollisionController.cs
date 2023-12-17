using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPlayerCollisionController : MonoBehaviour
{
    [SerializeField] private NPlayerLevelFollow levelPlayer;
    [SerializeField] private NPlayerOpenControl openPlayer;

    [SerializeField] private NPlayerScriptableObject _stats;
    [SerializeField] private SoundPlayerScriptableObject _sounds;
    public AudioSource MainSounds;
    [Space]
    [Header("Collection Data")]
    public CollectablesData blueChip;
    public CollectablesData starChip;
    public CollectablesData yellowRing;
    public CollectablesData halfRing;
    public CollectablesData powerRing;
    public CollectablesData spikeRing;
    public CollectablesData greenRing;

    void OnTriggerEnter(Collider other)
    {
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
            levelPlayer.currentScore += (int)(data.Score * (data.timesLink ? levelPlayer.link : 1));
            if(data.increaseLink) levelPlayer.LinkIncrease();
            else if(data.clearLink) levelPlayer.LinkEmpty();
            levelPlayer.LevelTimeLeft += data.Time;
            levelPlayer.currentChips += data.Chips;
        }
        else if(openPlayer != null)
        {
            _stats.openChips += data.Chips;
        }

        _stats.PowerBuff = data.givePower;
        _stats.PowerBuffTimeLeft = data.powerTime;
        _stats.BoostGauge += data.Boost;

        // if(data.chipItem) pointItemScript.InstantiatePointAndChip(true);
        if(data.instantOff) other.gameObject.SetActive(false);
        MainSounds.PlayOneShot(data.interactionSound, 1.0f);
    }
}
