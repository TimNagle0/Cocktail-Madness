using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shaker : MonoBehaviour
{
    [SerializeField] public KeyCode shakeKey;
    [SerializeField] public KeyCode trashKey;

    public IngredientList IngredientList;
    public List<GameObject> currentIngredients = new List<GameObject>();
    public Recipe currentRecipe;

    public float currentShakeTime = 0;

    private Vector3 lastPosition;
    private Collider2D col;
    private AudioSource audioSource;

    public enum ShakerState
    {
        preparing,
        canShake,
        serving
    }

    public ShakerState shakerState = ShakerState.preparing;
    public bool isShaking;
    private bool wasShaking = false;

    //The time interval that the player has between pressing space
    [SerializeField] private float shakeTimeOutTime = 1f;
    [SerializeField] private float shakeTimeMultiplier = 50f;
    private float shakeSessionTime = 0;
    public float minMoveDistance;

    public float distance;

    public event Action resetShaker;
    public event Action trashShaker;
    public event Action startShaking;
    public event Action stopShaking;
    public event Action<BoxCollider2D> serveCustomer;

    
    void Start()
    {
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        SubscribeToEvents();
        currentRecipe = gameObject.AddComponent<Recipe>();
        lastPosition = transform.position;
    }

    private void SubscribeToEvents()
    {
        IngredientList.useIngredient += UseIngredient;
    }


    void Update()
    {
        isShaking = Input.GetKeyDown(shakeKey);
        if (Input.GetKeyDown(trashKey))
        {
            trashShaker();
            ResetShaker();
        }

        // Checks if the player was already shaking and if they continue to do so within the allowed interval
        // Starts with a set amount and adds time to the shakebar based on how fast the player presses the hotkey
        if (isShaking && !wasShaking)
        {
            startShaking();
            audioSource.Play();
            currentShakeTime += 0.25f;
            wasShaking = isShaking;
            shakeSessionTime = Time.time;
        }
        else if(isShaking)
        {
            float time = Mathf.Clamp01(1 / ((Time.time-shakeSessionTime)*shakeTimeMultiplier));
            currentShakeTime += time;
            shakeSessionTime = Time.time;
            
        }
        else if (!isShaking && wasShaking && Time.time - shakeTimeOutTime > shakeSessionTime || Input.GetKeyDown(trashKey))
        {
            stopShaking();
            audioSource.Stop();
            wasShaking = isShaking;
            
        }
    }

    //If the shaker is moved, check if they are trashing or serving it, otherwise it will snap back to the start
    private void OnMouseUp()
    {
        Collider2D[] overlap = Physics2D.OverlapAreaAll(col.bounds.min, col.bounds.max);
        if (overlap.Length > 1)
        {
            foreach (Collider2D c in overlap)
            {
                if (c.gameObject.tag == "Trash")
                {
                    trashShaker();
                    ResetShaker();
                    return;
                }
                if(c.gameObject.tag == "Serving location")
                {
                    serveCustomer(c.GetComponent<BoxCollider2D>());
                }
            }

        }
    }

    public Recipe CreateOrder()
    {

        currentRecipe.Ingredients = currentIngredients;
        currentRecipe.shakeTime = currentShakeTime;
        shakerState = ShakerState.preparing;
        return currentRecipe;
    }

    public void EnableShakeTimer()
    {
        shakerState = ShakerState.canShake;
    }
    public void ResetShaker()
    {
        resetShaker();
        shakerState = ShakerState.preparing;
        currentIngredients.Clear();
        currentShakeTime = 0f;
    }

    public void EnableServingMode()
    {
        shakerState = ShakerState.serving;
    }

    private void GetPositionDifference()
    {
        Vector3 currentPosition = transform.position;
        distance = Mathf.Abs(Vector3.Distance(currentPosition, lastPosition));
        isShaking = Mathf.Abs(Vector3.Distance(currentPosition, lastPosition)) > minMoveDistance;
        lastPosition = currentPosition;
    }

    #region events

    public void UseIngredient(GameObject ingredient)
    {
        currentIngredients.Add(ingredient);
        ingredient.GetComponent<Ingredient>().PlaySound();
    }

    
    #endregion
}
