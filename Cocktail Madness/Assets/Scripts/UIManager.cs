using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public ShakeTimeBar shakeTimeBar;
    public IngredientTracker ingredientTracker; 
    public Shaker shaker;
    public OrderFeedbackText orderFeedbackText;
    public UILives lives;
    public UILevelTimer levelTimer;

    public Text Served;
    public Text PerfectServed;

    public GameOverScreen gameOver;


    private float lastTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameOver.gameObject.SetActive(false);
        UpdateServed(0);
        shaker.resetShaker += ClearIngredientTracker;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateShakeTimeBar();
    }

    public void SetLevelTimer(float time)
    {
        levelTimer.SetTimer(time);
    }

    public void EnableLevelTimer(bool isEnabled)
    {
        levelTimer.gameObject.SetActive(isEnabled);
    }


    public void TakeDamage()
    {
        lives.TakeDamage();
    }

    public void UpdateServed(int served)
    {
        Served.text = served.ToString();
    }

    public void UpdatePerfectServed(int served)
    {
        PerfectServed.text = served.ToString();
    }



    public void UpdateServingMessage(bool isCorrect, string scenario)
    {
        orderFeedbackText.DisplayMessage(isCorrect, scenario);
    }

    void ClearIngredientTracker()
    {
        ingredientTracker.ClearIngredientList();
    }
    void UpdateShakeTimeBar()
    {
        if(shaker.currentShakeTime != lastTime)
        {
            shakeTimeBar.SetCurrentTime(shaker.currentShakeTime);
            lastTime = shaker.currentShakeTime;
        }
    }
    
}
