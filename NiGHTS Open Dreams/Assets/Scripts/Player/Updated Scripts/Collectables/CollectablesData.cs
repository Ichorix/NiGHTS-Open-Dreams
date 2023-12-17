using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectablesData", menuName = "ScriptableObjects/Collectable")]
public class CollectablesData : ScriptableObject
{
    [Tooltip("How much score is given when you collect this item")]
    public float Score;
    [Tooltip("Should the score be multiplied by the link?")]
    public bool timesLink;
    [Tooltip("Should collecting this item increase the link?")]
    public bool increaseLink;
    [Tooltip("Should collecting this item clear the link?")]
    public bool clearLink;
    [Tooltip("How much boost will be added when you collect this item")]
    public float Boost;
    [Tooltip("How much time is given when you collect this item")]
    public float Time;
    [Tooltip("Should this item  give you the Power buff?")]
    public bool givePower;
    [Tooltip("If givePower is true, How long does it last?")]
    public float powerTime;
    [Tooltip("How many chips should be given when you collect this item")]
    public int Chips;
    [Tooltip("Should collecting this item spawn a chip item on the screen?")]
    public bool chipItem;
    [Tooltip("Should this item be turned off immediately? Rings should have this off, chips have this on")]
    public bool instantOff;
    [Space]
    [Tooltip("The sound that plays when you collect this item")]
    public AudioClip interactionSound;
    
}
