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


    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip timeOverSound;
    [SerializeField] private GameObject playAgainButton;
    public void ShowGameOverScreen()
    {
        PlayerStats.timePlayed = Time.time;
        audioSource = GetComponentInChildren<AudioSource>();
        
        int perfect = PlayerStats.perfectServings;
        int correct = PlayerStats.correctServings;
        int incorrect = PlayerStats.incorrectServings;

        PauseControl.PauseGame();
        gameObject.SetActive(true);
        totalServings.text = string.Format("{0} servings", PlayerStats.GetTotalServings());
        perfectServings.text = perfect.ToString();
        correctServings.text = correct.ToString();
        incorrectServings.text = incorrect.ToString();
        totalScore.text = PlayerStats.GetTotalScore().ToString();

        if(PlayerStats.lives <= 0)
        {
            gameOverText.sprite = gameOver;
            StartCoroutine(playSound(gameOverSound));
        }
        else
        {
            gameOverText.sprite = timeOver;
            StartCoroutine(playSound(timeOverSound));
        }
        if(Time.time > 0)
        {
            playAgainButton.SetActive(false);
        }

        
    }

    IEnumerator playSound(AudioClip clip)
    {
        yield return new WaitUntil(() => audioSource.isActiveAndEnabled);
        audioSource.PlayOneShot(clip);
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
