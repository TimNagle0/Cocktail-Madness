using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddIngredientBar : MonoBehaviour
{
    [SerializeField] Slider slider;

    bool isRunning = false;
    private float startTime;
    private float timer;

    private void Awake()
    {
        slider.value = slider.minValue;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isRunning)
        {
            if (Time.time - startTime > timer)
            {
                ResetSlider();
            }
            else
            {
                slider.value += Time.deltaTime;
            }
        }
    }
    private void ResetSlider()
    {
        isRunning = false;
        slider.value = slider.minValue;
        gameObject.SetActive(false);
    }

    public void StartSlider(float time)
    {
        gameObject.SetActive(true);
        isRunning = true;
        timer = time;
        startTime = Time.time;
    }



}
