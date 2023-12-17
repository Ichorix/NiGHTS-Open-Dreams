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

    [Space]
    [Header("Save Data")]
    [Tooltip("The highest Grade that has been saved for this level")]
    public int SavedGrade = 0;
    [Tooltip("The highest Score that has been saved for this level")]
    public float SavedScore = 0;
}
