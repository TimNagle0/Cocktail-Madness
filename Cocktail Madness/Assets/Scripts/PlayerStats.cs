using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public static int lives;
    public static int correctServings;
    public static int perfectServings;
    public static int incorrectServings;
    public static float timePlayed;


    public static bool isAddingIngredient;
    private static float startTime;
    // Start is called before the first frame update
    void Start()
    {
        timePlayed = 0f;
        perfectServings = 0;
        lives = 3;
        incorrectServings = 0;
        correctServings = 0;
        isAddingIngredient = false;
    }

    private void Update()
    {
        if (isAddingIngredient)
        {
            if(Time.time - startTime > 1)
            {
                isAddingIngredient = false;
            }
        }
    }

    public static int GetTotalServings()
    {
        return perfectServings + correctServings + incorrectServings;
    }


    public static int GetTotalScore()
    {
        return (perfectServings * 2 * 100) + (correctServings * 100);
    }

    public static void AddIngredient()
    {
        isAddingIngredient = true;
        startTime = Time.time;
    }
    private static void stopAdding()
    {
        isAddingIngredient = false;
    }

}
