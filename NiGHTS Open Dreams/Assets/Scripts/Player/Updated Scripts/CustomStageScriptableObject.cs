using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomStageScriptableObject", menuName = "ScriptableObjects/New Stage")]
public class CustomStageScriptableObject : ScriptableObject
{
    public AnimationCurve[] Grades = new AnimationCurve[4];
    public float[] Times = new float[4];
}
