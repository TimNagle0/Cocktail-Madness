using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    

    


    [Header("Needed components")]
    public UIManager uiManager;
    public CustomerManager customerManager;

    private float startTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        uiManager.EnableLevelTimer(true);
        startTime = Time.time;
        
        customerManager.SetCustomerSettings(customerOrderTime, customerOrderVariance);
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > totalTime)
        {
            uiManager.gameOver.ShowGameOverScreen(); 
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
            LoadNextLevel();
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
