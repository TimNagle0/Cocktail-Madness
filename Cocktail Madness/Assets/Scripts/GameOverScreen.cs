using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public Text totalServings;
    public Text perfectServings;
    public Text correctServings;
    public Text incorrectServings;
    public Text totalScore;

    public Image gameOverText;
    public Sprite gameOver;
    public Sprite timeOver;

    public void ShowGameOverScreen()
    {
        PlayerStats.timePlayed = Time.time;
        
        int perfect = PlayerStats.perfectServings;
        int correct = PlayerStats.correctServings;
        int incorrect = PlayerStats.incorrectServings;

        PauseControl.PauseGame();

        totalServings.text = string.Format("{0} servings", PlayerStats.GetTotalServings());
        perfectServings.text = perfect.ToString();
        correctServings.text = correct.ToString();
        incorrectServings.text = incorrect.ToString();
        totalScore.text = PlayerStats.GetTotalScore().ToString();

        if(PlayerStats.lives <= 0)
        {
            gameOverText.sprite = gameOver;
        }
        else
        {
            gameOverText.sprite = timeOver;
        }

        gameObject.SetActive(true);
    }

    public void RedirectBof()
    {
        //
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
