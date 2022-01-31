using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ingredient : MonoBehaviour
{
    [SerializeField] public KeyCode hotkey;

    // Events for different actions for data collection,
    // OnUse is also used for adding ingredients to the shaker
    public event Action<GameObject> OnSelect;
    public event Action<GameObject> OnDeselect;
    public event Action<GameObject> OnUse;

    private Collider2D col;
    private AudioSource audioSource;

    void Start()
    {
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(hotkey) && !PlayerStats.isAddingIngredient)
        {
            PlayerStats.AddIngredient();
            OnUse(gameObject);
        }
    }
    
    // Code for adding ingredients by dragging them to the shaker.
    // Also detects if a player picks up an ingredient but drops is again for data collection
    private void OnMouseDown()
    {
        OnSelect(gameObject);
    }
    private void OnMouseUp()
    {
        Collider2D[] overlap = Physics2D.OverlapAreaAll(col.bounds.min, col.bounds.max);
        if (overlap.Length > 1)
        {
            foreach(Collider2D c in overlap)
            {
                if(c.gameObject.name == "Shaker")
                {
                    //OnUse(gameObject);
                    return;
                }
            }

        }
        OnDeselect(gameObject);  
    } 

    public void PlaySound()
    {
        audioSource.Play();
    }
}
