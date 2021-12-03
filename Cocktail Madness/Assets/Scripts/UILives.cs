using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILives : MonoBehaviour
{
    public Sprite fullHeart;
    public Sprite emptyHeart;
    private Image[] hearts;


    // Start is called before the first frame update
    void Start()
    {
        hearts = GetComponentsInChildren<Image>();
        ResetLives();
    }

    public void ResetLives()
    {
        foreach(Image h in hearts)
        {
            h.sprite = fullHeart;
        }
    }

    public void TakeDamage()
    {
        for(int i = hearts.Length-1; i >= 0; i--)
        {
            if(hearts[i].sprite == fullHeart)
            {
                hearts[i].sprite = emptyHeart;
                break;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
