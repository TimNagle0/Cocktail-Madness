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
    // Start is called before the first frame update
    void Start()
    {
        timePlayed = 0f;
        perfectServings = 0;
        lives = 3;
        incorrectServings = 0;
        correctServings = 0;
    }

    public static int GetTotalServings()
    {
        return perfectServings + correctServings + incorrectServings;
    }


}
