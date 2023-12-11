using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPlayerCollisionController : MonoBehaviour
{
    [SerializeField]
    private NPlayerLevelFollow levelPlayer;
    [SerializeField]
    private NPlayerOpenControl openPlayer;

    [SerializeField]
    private NPlayerScriptableObject _stats;
    [SerializeField]
    private SoundPlayerScriptableObject _sounds;
    public AudioSource MainSounds;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("lb_groundTarget"))
        {
            if(openPlayer != null)
            {
                openPlayer.BumpUpFromGround();
            }
        }

        
        if(other.CompareTag("BlueChip"))
        {
            CollectBlueChip(other);
        }
        if(other.CompareTag("Star"))
        {
            CollectStarChip(other);
        }
        if(other.CompareTag("YellowRing"))
        {
            MainSounds.PlayOneShot(_sounds.YellowRingSFX, 1.0f);
            if(_stats.boostGauge < 90) _stats.boostGauge += 10;
            else _stats.boostGauge = 100;


            if(levelPlayer != null)
                levelPlayer.currentScore += 10;
        }
        if(other.CompareTag("GreenRing"))
        {
            MainSounds.PlayOneShot(_sounds.GreenRingSFX, 1.0f);
            if(_stats.boostGauge < 90) _stats.boostGauge += 10;
            else _stats.boostGauge = 100;
        }
        if(other.CompareTag("HalfRing"))
        {
            MainSounds.PlayOneShot(_sounds.HalfRingSFX, 1.0f);
            if(_stats.boostGauge <= 90) _stats.boostGauge += 10;
            else _stats.boostGauge = 100;

            if(levelPlayer != null)
                levelPlayer.currentScore += 10;
        }
        if(other.CompareTag("PowerRing"))
        {
            MainSounds.PlayOneShot(_sounds.PowerRingSFX, 1.0f);
            _stats.boostGauge = 100;
            //power = true;
        }
        if(other.CompareTag("SpikeRing"))
        {
            MainSounds.PlayOneShot(_sounds.SpikeRingSFX, 1.0f);
            _stats.boostGauge -= 5;
        }
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
        }
    }
    void OnCollisionExit(Collision other)
    {
        if(other.gameObject.CompareTag("lb_groundTarget") && openPlayer != null)
        {
            openPlayer._animations.Grounded = false;
        }
    }

    public void CollectBlueChip(Collider other)
    {
        //pointItemScript.InstantiatePointAndChip(true);
        MainSounds.PlayOneShot(_sounds.BlueChipSFX, 1.0f);
        other.gameObject.SetActive(false);

        if(levelPlayer != null)
            levelPlayer.currentChips += 1;
        else _stats.openChips += 1;
    }
    public void CollectStarChip(Collider other)
    {
        MainSounds.PlayOneShot(_sounds.BlueChipSFX, 1.0f);
        other.gameObject.SetActive(false);
        levelPlayer.currentScore += 10;
        //levelTimeLeft += 0.25f;
    }
}
