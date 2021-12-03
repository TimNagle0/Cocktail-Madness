using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour
{
    public static bool gameIsPaused;
    // Start is called before the first frame update
    void Awake()
    {
        UnPauseGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void PauseGame()
    {
        gameIsPaused = true;
        Time.timeScale = 0f;
    }

    public static void UnPauseGame()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;
    }
}
