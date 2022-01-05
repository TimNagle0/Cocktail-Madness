using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelManager : MonoBehaviour
{
    [Header ("Adjust these values to tweak the initial difficulty of the level")]
    public float totalTime;

    public float customerOrderTime;
    public float customerOrderVariance;
    public float customerInterval;

    [Header("Adjust these values to tweak the increasing difficulty")]
    [Range(1, 2)]
    public float orderTimeMultiplier;

    [Range(1, 2)]
    public float intervalMultiplier;

    public float difficultyInterval;
    public bool isTutorial;


    public event Action<bool> finishTutorial;
    private bool isFinished = false;


    [Header("Needed components")]
    public UIManager uiManager;
    public CustomerManager customerManager;


    [Header("Background music clips")]
    [SerializeField] private List<AudioClip> clips = new List<AudioClip>();
    [SerializeField] private float hurryTime;
    private bool isPlayingHurry = false;
    private AudioSource audioSource;

    private float startTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        uiManager.EnableLevelTimer(true);
        startTime = Time.time;
        audioSource = GetComponentInChildren<AudioSource>();
        customerManager.SetCustomerSettings(customerOrderTime, customerOrderVariance);
        if (isTutorial)
        {
            audioSource.clip = clips[0];
            uiManager.EnableLevelTimer(false);
            customerManager.SpawnTutorialCustomer();
        }
        else
        {
            audioSource.clip = clips[1];
            uiManager.SetLevelTimer(totalTime);
            customerManager.SpawnCustomers(customerInterval);
        }
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > totalTime)
        {
            if (!uiManager.gameOver.gameObject.activeSelf)
            {
                uiManager.gameOver.ShowGameOverScreen();
            }
            
        }

        if(PlayerStats.GetTotalServings() > difficultyInterval)
        {
            //Increase difficulty
            customerOrderTime /= orderTimeMultiplier;
            customerInterval /= intervalMultiplier;
            difficultyInterval += difficultyInterval;
        }

        if(isTutorial && PlayerStats.correctServings + PlayerStats.perfectServings >= 1)
        {
            if (!isFinished)
            {
                isFinished = true;
                //finishTutorial(false);
                uiManager.ShowEndTutorial();
            }
        }
        if (isTutorial)
            return;

        if (Time.timeSinceLevelLoad - hurryTime > startTime && !isPlayingHurry)
        {
            audioSource.clip = clips[2];
            isPlayingHurry = true;
            audioSource.Play();
        }


    }
    


    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
