using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiGHTSAnimations : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject player;
    [Header("Real Targets")]
    public Transform ActiveTargets;
    public GameObject real_Waist, real_LeftFoot, real_LeftFootHint, real_RightFoot, real_RightFootHint, real_Chest1, real_Chest2, real_Neck, real_Head, real_LeftHat, real_LeftHatHint, real_RightHat, real_RightHatHint, real_LeftHand, real_LeftHandHint, real_RightHand, real_RightHandHint;
    [Header("Flying Targets")]
    public Transform FlyingForwardTargets;
    public GameObject flying_Waist, flying_LeftFoot, flying_LeftFootHint, flying_RightFoot, flying_RightFootHint, flying_Chest1, flying_Chest2, flying_Neck, flying_Head, flying_LeftHat, flying_LeftHatHint, flying_RightHat, flying_RightHatHint, flying_LeftHand, flying_LeftHandHint, flying_RightHand, flying_RightHandHint;
    [Header("Idle Targets")]
    public Transform IdleTargets;
    public GameObject idle_Waist, idle_LeftFoot, idle_LeftFootHint, idle_RightFoot, idle_RightFootHint, idle_Chest1, idle_Chest2, idle_Neck, idle_Head, idle_LeftHat, idle_LeftHatHint, idle_RightHat, idle_RightHatHint, idle_LeftHand, idle_LeftHandHint, idle_RightHand, idle_RightHandHint;

    void Update()
    {
        transform.position = player.transform.position;
    }
}
