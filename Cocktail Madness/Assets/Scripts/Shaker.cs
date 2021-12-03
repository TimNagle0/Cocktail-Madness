using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shaker : MonoBehaviour
{
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
    private float shakeCorrectionTime = 0.1f;
    private float shakeSessionTime = 0;
    public float minMoveDistance;

    public float distance;

    public event Action resetShaker;
    public event Action<BoxCollider2D> serveCustomer;

    

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        if(shakerState == ShakerState.canShake)
        {
            GetPositionDifference();
        }
        
        if (isShaking && !wasShaking)
        {
            audioSource.Play();
            currentShakeTime += Time.deltaTime;
            wasShaking = isShaking;
            shakeSessionTime = Time.time;
        }
        else if(isShaking)
        {
            shakeSessionTime = Time.time;
            currentShakeTime += Time.deltaTime;
        }else if (!isShaking && wasShaking && Time.time - shakeCorrectionTime > shakeSessionTime)
        {
            audioSource.Stop();
            wasShaking = isShaking;
            
        }
    }

    private void OnMouseUp()
    {
        Collider2D[] overlap = Physics2D.OverlapAreaAll(col.bounds.min, col.bounds.max);
        if (overlap.Length > 1)
        {
            foreach (Collider2D c in overlap)
            {
                if (c.gameObject.tag == "Trash")
                {
                    ResetShaker();
                    return;
                }
                if(c.gameObject.tag == "Serving location" && shakerState == ShakerState.serving)
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
        //Debug.Log("Used ingredient : " + ingredient.name);
    }

    
    #endregion
}
