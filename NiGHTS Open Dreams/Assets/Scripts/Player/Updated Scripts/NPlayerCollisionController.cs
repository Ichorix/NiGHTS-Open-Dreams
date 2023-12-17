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
            _stats.BoostGauge += 10;

            if(levelPlayer != null)
            {
                levelPlayer.currentScore += 10 * levelPlayer.link;
                levelPlayer.LinkIncrease();
            }
        }
        if(other.CompareTag("GreenRing"))
        {
            MainSounds.PlayOneShot(_sounds.GreenRingSFX, 1.0f);
            _stats.BoostGauge += 10;
        }
        if(other.CompareTag("HalfRing"))
        {
            MainSounds.PlayOneShot(_sounds.HalfRingSFX, 1.0f);
            _stats.BoostGauge += 10;

            if(levelPlayer != null)
            {
                levelPlayer.currentScore += 10 * levelPlayer.link;
                levelPlayer.LinkIncrease();
            }
        }
        if(other.CompareTag("PowerRing"))
        {
            MainSounds.PlayOneShot(_sounds.PowerRingSFX, 1.0f);
            _stats.PowerBuffTimeLeft = 10;
            _stats.PowerBuff = true;
            if(levelPlayer != null)
            {
                levelPlayer.currentScore += 10 * levelPlayer.link;
                levelPlayer.LinkIncrease();
            }
        }
        if(other.CompareTag("SpikeRing"))
        {
            MainSounds.PlayOneShot(_sounds.SpikeRingSFX, 1.0f);
            _stats.BoostGauge -= 5;
            if(levelPlayer != null)
                levelPlayer.LinkEmpty();
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
        if(levelPlayer != null)
        {
            levelPlayer.LinkIncrease();
            levelPlayer.currentScore += 3 * levelPlayer.link;
            levelPlayer.LevelTimeLeft += 0.3f;
        }
    }
}
