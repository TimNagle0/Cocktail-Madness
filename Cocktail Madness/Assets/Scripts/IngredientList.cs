using System;
using System.Collections.Generic;
using UnityEngine;

public class IngredientList : MonoBehaviour
{
    static public List<GameObject> ingredients = new List<GameObject>();

    public event Action<GameObject> useIngredient;
    // Start is called before the first frame update
    private void Awake()
    {
        ingredients.Clear();
        Ingredient[] i = GetComponentsInChildren<Ingredient>();
        foreach (Ingredient ingredient in i)
        {
            ingredients.Add(ingredient.gameObject);
        }
        CreateIngredientEvents();


    }

    private void CreateIngredientEvents()
    {
        foreach (GameObject ingredient in ingredients)
        {
            Ingredient i = ingredient.GetComponent<Ingredient>();
            i.OnDeselect += DeselectIngredient;
            i.OnSelect += SelectIngredient;
            i.OnUse += UseIngredient;
        }
    }
    private void DeselectIngredient(GameObject ingredient)
    {
        //Debug.Log("deselected ingredient : " + ingredient.name);
    }

    private void SelectIngredient(GameObject ingredient)
    {
        //Debug.Log("Selected ingredient : " + ingredient.name);
    }

    private void UseIngredient(GameObject ingredient)
    {
        useIngredient(ingredient);
    }

    // Update is called once per frame
    void Update()
    {

    }
}