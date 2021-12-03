using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelTimer : MonoBehaviour
{
    public Text timerText;
    private float currentTime = 0;
    private bool isRunning = false;
    public void SetTimer(float time)
    {
        currentTime = time;
        isRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning)
            return;
        if(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            currentTime = 0;
            isRunning = false;
        }

        DisplayTime(currentTime);
    }

    private void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
