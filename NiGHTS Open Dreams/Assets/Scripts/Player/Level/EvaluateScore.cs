using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaluateScore : MonoBehaviour
{
    public Button bossFightButton;
    private AnimationCurve curve;
    public AnimationCurve course1, course2, course3, course4;
    private float grade;
    private float grade1, grade2, grade3, grade4;

    void OnEnable()
    {
        grade1 = 0;
        grade2 = 0;
        grade3 = 0;
        grade4 = 0;
    }
    // Returns a grade between 0 - 5, 0 = F and 5 = A
    // A B C D E F
    public int CalculateGrade(int course, float score)
    {
        //Get course num for curve
        if (course == 1) curve = course1;
        if (course == 2) curve = course2;
        if (course == 3) curve = course3;
        if (course == 4) curve = course4;

        grade = curve.Evaluate(score);
        Debug.Log("Segment Grade = " + (int)grade);

        //Get course num to save grade
        if (course == 1) grade1 = grade ;
        if (course == 2) grade2 = grade ;
        if (course == 3) grade3 = grade ;
        if (course == 4) grade4 = grade ;

        return (int)grade;
    }

    public int CalculateFullGrade()
    {
        float fullGrade = (grade1 + grade2 + grade3 + grade4 + 2) * 0.25f;
        Debug.Log("Full Grade = " + (int)fullGrade);
        UnlockBossfight(fullGrade);
        return (int)fullGrade;
    }

    /*
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K)) CalculateGrade(1, testScore);
        if(Input.GetKeyDown(KeyCode.O)) CalculateFullGrade();
    }*/

    void UnlockBossfight(float fullGrade)
    {
        if (fullGrade >= 4) bossFightButton.interactable = true;
    }
}
