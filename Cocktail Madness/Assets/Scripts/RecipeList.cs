using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeList : MonoBehaviour
{
     //List<Recipe> recipeList = new List<Recipe>();
    static public Recipe[] recipeList;
    private void Awake()
    {
        recipeList = GetComponentsInChildren<Recipe>();
        
    }


}
