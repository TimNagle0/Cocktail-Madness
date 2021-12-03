using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientTracker : MonoBehaviour
{
    public GameObject trackerImagePrefab;
    public IngredientList ingredientList;
    private List<GameObject> images = new List<GameObject>();
    public Dictionary<GameObject,int> tracker = new Dictionary<GameObject,int>();

    public void AddIngredient(GameObject ingredient)
    {
        GameObject newImage = Instantiate(trackerImagePrefab, transform);
        newImage.GetComponent<Image>().sprite = ingredient.GetComponent<SpriteRenderer>().sprite;
        images.Add(newImage);
    }

    public void ClearIngredientList()
    {
        foreach(GameObject ingredient in images)
        {
            Destroy(ingredient);
        }
        images.Clear();
    }
    // Start is called before the first frame update
    void Start()
    {
        ingredientList.useIngredient += AddIngredient;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
