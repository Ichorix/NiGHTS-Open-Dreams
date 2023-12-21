using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomStageScriptableObject", menuName = "ScriptableObjects/New Stage")]
public class CustomStageScriptableObject : ScriptableObject
{
    [Header("Level Data")]
    [Tooltip("The curve that will be evaluated into score. Y axis from F = 0 to A = 5. X axis is inputted score")]
    public AnimationCurve[] Grades = new AnimationCurve[4];
    [Tooltip("The amount of time given per track")]
    public float[] Times = new float[4];
    [Tooltip("The amount of chips needed to move on to the next track")]
    public int[] ChipsRequired = new int[4];
}
