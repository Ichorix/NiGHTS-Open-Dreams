using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundPlayerScriptableObject", menuName = "ScriptableObjects/SoundManager")]
public class SoundPlayerScriptableObject : ScriptableObject
{
    [Header("Player Sounds")]
    public AudioClip BoostStartSFX;
    public AudioClip BoostIngSFX;
    public AudioClip BoostEndSFX;
    [Header("Interaction Sounds")]
    public AudioClip BlueChipSFX;
    public AudioClip YellowRingSFX;
    public AudioClip GreenRingSFX;
    public AudioClip HalfRingSFX;
    public AudioClip PowerRingSFX;
    public AudioClip SpikeRingSFX;
}
