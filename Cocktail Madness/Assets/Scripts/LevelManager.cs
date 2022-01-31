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

    [Header("Adjust these value to tweak the end game difficulty")]
    [SerializeField] private float minOrderTime = 5;
    [SerializeField] private float gameTimeTreshold =0;
    private float difficultyStep = 10;


    public event Action<bool> finishTutorial;
    private bool isFinished = false;


    [Header("Needed components")]
    public UIManager uiManager;
    public CustomerManager customerManager;
    [SerializeField] GameObject HotkeyOverlay;


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
        audioSource.clip = clips[0];
        if (isTutorial)
        {  
            uiManager.EnableLevelTimer(false);
            customerManager.SpawnTutorialCustomer();
        }
        else
        {
            uiManager.SetLevelTimer(totalTime);
            customerManager.SpawnCustomers(customerInterval);
        }
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > gameTimeTreshold && customerOrderTime > minOrderTime)
        {
            customerOrderTime = Mathf.Lerp(customerOrderTime, minOrderTime, Time.deltaTime * Time.time / 2500); //0.04%
        }
        if (Time.time - startTime > totalTime && !isTutorial)
        {
            if (!uiManager.gameOver.gameObject.activeSelf)
            {
                uiManager.gameOver.ShowGameOverScreen();
                HotkeyOverlay.SetActive(false);
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
                HotkeyOverlay.SetActive(false);
            }
        }
        if (isTutorial)
            return;
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
