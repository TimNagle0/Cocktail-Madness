using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakeTimeBar : MonoBehaviour
{
    public Slider slider;
    public float maxValue;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = maxValue;
        slider.value = slider.minValue;
    }

    public void SetCurrentTime(float time)
    {
        if (time < maxValue)
        {
            slider.value = time;
        }
        else
        {
            slider.value = maxValue;
        }
        
    }

}
