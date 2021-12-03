using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ingredient : MonoBehaviour
{
    public event Action<GameObject> OnSelect;
    public event Action<GameObject> OnDeselect;
    public event Action<GameObject> OnUse;

    private Vector3 startPosition;
    private Collider2D col;
    private AudioSource audioSource;

    public bool isSelected = false;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
    }
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
                    OnUse(gameObject);
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
