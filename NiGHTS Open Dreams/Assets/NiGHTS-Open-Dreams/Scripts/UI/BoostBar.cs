using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;

    public Image fill;
    
    public void SetMaxBoost(int boost)
    {
        slider.maxValue = 100;
        slider.value = boost;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetBoost(int boost)
    {
        slider.value = boost;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
