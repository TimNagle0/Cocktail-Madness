using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderFeedbackText : MonoBehaviour
{
    public Color correctColor = new Color(0, 0.68f, 0);
    public Color incorrectColor = new Color(0.68f, 0, 0);

    public string correctMessage = "Correct!! the customer is happy!";
    public string incorrectIngredient = "Oops!? Those are not the right ingredients.";
    public string incorrectShake = "Oops!? This is not shaken enough.";


    private Dictionary<string, string> textCombination = new Dictionary<string, string>();
    private Text text;
    private void Start()
    {
        textCombination.Add("correct", correctMessage);
        textCombination.Add("ingredients", incorrectIngredient);
        textCombination.Add("shaketime", incorrectShake);
        text = GetComponent<Text>();
    }

    public void DisplayMessage(bool correct, string scenario)
    {
        if (correct)
        {
            text.color = correctColor;
        }
        else
        {
            text.color = incorrectColor;
        }
        text.text = textCombination[scenario];
        Invoke("ClearMessage", 2);
    }

    private void ClearMessage()
    {
        text.text = "";
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
