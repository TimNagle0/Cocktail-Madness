using System.Runtime.InteropServices;
using UnityEngine;

using System.Collections.Generic;

public class FirebaseHandler : MonoBehaviour
{
    // Set up the plugin functions from the firebase plugin
    [DllImport("__Internal")]
    public static extern void ListenForValueChanged(string path, string objectName, string callback, string fallback);

    [DllImport("__Internal")]
    public static extern void PostJSON(string path, string value, string callback, string fallback);


    #region json serializers

    //Set up the classes which will store the json data from the database
    public class ShakerSerializer
    {
        public string shakeTime;
    }
    

    public class IngredientSerializer
    {
        public string Drink_01;
        public string Drink_02;
        public string Drink_03;
        public string Lime_slices;
        public string Orange_slices;
        public string Ice_cubes;
    }

    public class ServingSpotSerializer
    {
        public string Spot_01;
        public string Spot_02;
    }
    ShakerSerializer shakerSerializer = new ShakerSerializer();
    ServingSpotSerializer servingSpotSerializer = new ServingSpotSerializer();
    IngredientSerializer newIngredients = new IngredientSerializer();
    IngredientSerializer lastIngredients = new IngredientSerializer();
    #endregion



    private bool isStartup = false;
    public IngredientTracker tracker;
    public Shaker shaker;
    public OrderManager orderManager;

    private string uid = "5DG2x5IYTBS6uvZvbUKSUkDUbEO2";
    private string shakerPath;
    //Setup all the listeners for changes in the firebase database
    // Also reset any existing data and set up a listener for the shaker reset event
    void Start()
    {
        shakerPath = "UsersData/" + uid + "/ShakerState";
        shaker.resetShaker += ResetAll;
        isStartup = true;
        ResetAll();
        //ListenForValueChanged(shakerPath, gameObject.name, "onSuccess", "onFailure");
        //ListenForValueChanged("stateShaker", gameObject.name, "onSuccess", "onFailure");
        //ListenForValueChanged("Ingredients", gameObject.name, "CheckIngredients", "onFailure");
        //ListenForValueChanged("ServingSpots", gameObject.name, "ServeCocktail", "onFailure");
        //ListenForValueChanged("stateShaker/shakeTime", gameObject.name, "onSuccess", "onFailure");
        //onSuccess("1");
        isStartup = false;
    }

    public void PostResult(string data) {
        Debug.Log(data);
    }
    public void ResetShakeTime()
    {
        PostJSON(shakerPath + "/shakeTime", "false", "PostResult", "onFailure");
        PostJSON("stateShaker/shakeTime", "false", "PostResult", "onFailure");
    }
    public void ResetIngredients()
    {
        SetupEmptyIngredients();
        foreach (var ingredient in lastIngredients.GetType().GetFields())
        {
            PostJSON("Ingredients/" + ingredient.Name, "0", "PostResult", "onFailure");
        }
    }

    private void ResetAll()
    {
        //ResetIngredients();
        ResetShakeTime();
        //ResetServingSpots();
    }

    public void ResetServingSpots()
    {
        PostJSON("ServingSpots/Spot_01", "false", "PostResult", "onFailure");
        PostJSON("ServingSpots/Spot_02", "false", "PostResult", "onFailure");

    }
    
    void SetupEmptyIngredients()
    {
        lastIngredients.Drink_01 = "0";
        lastIngredients.Drink_02 = "0";
        lastIngredients.Drink_03 = "0";
        lastIngredients.Ice_cubes = "0";
        lastIngredients.Lime_slices = "0";
        lastIngredients.Orange_slices = "0";


    }
    private void onSuccess(string data)
    {
        shakerSerializer= JsonUtility.FromJson<ShakerSerializer>(data);
        bool isShaking = bool.Parse(shakerSerializer.shakeTime);
        shaker.isShaking = isShaking;
    }

    public void ServeCocktail(string data)
    {
        if(data == null)
        {
            return;
        }
        servingSpotSerializer = JsonUtility.FromJson<ServingSpotSerializer>(data);
        if (bool.Parse(servingSpotSerializer.Spot_01))
        {
            orderManager.CompareOrders(0);
            ResetServingSpots();
        }
        else if(bool.Parse(servingSpotSerializer.Spot_02))
        {
            orderManager.CompareOrders(1);
            ResetServingSpots();
        }
    }

    #region Adding ingredients

    private void CheckIngredients(string data)
    {
        if(isStartup)
        {
            //SetupEmptyIngredients();
        }
        else
        {
            newIngredients = JsonUtility.FromJson<IngredientSerializer>(data);
            string ingredientName = GetChangedIngredient(lastIngredients, newIngredients);
            AddIngredient(ingredientName);
            lastIngredients = newIngredients;
        }
    }
    public string GetChangedIngredient(IngredientSerializer oldIngredients, IngredientSerializer newIngredients)
    {
        foreach (var ingredient in oldIngredients.GetType().GetFields())
        {

            Debug.Log(ingredient.GetType());
            float oldvalue = float.Parse(ingredient.GetValue(oldIngredients).ToString());
            float newvalue = float.Parse(ingredient.GetValue(newIngredients).ToString());
            Debug.Log("Old value : " + oldvalue);
            Debug.Log("new value : " + newvalue);
            if (oldvalue != newvalue)
            {
                Debug.Log("Sending name : " + ingredient.Name);
                return ingredient.Name;
            }

            //string value = ingredient.GetValue(oldIngredients).ToString();
            //Debug.Log("Name : " + ingredient.Name + " value : " + value);
        }
        return "error: found no changed ingredient";
    }
    private void AddIngredient(string name)
    {
        Debug.Log("Adding ingredient with name : " +name);
        switch (name)
        {
            case "Drink_01":
                tracker.AddIngredient(IngredientList.ingredients[0]);
                shaker.UseIngredient(IngredientList.ingredients[0]);
                break;
            case "Drink_02":
                tracker.AddIngredient(IngredientList.ingredients[1]);
                shaker.UseIngredient(IngredientList.ingredients[1]);
                break;
            case "Drink_03":
                tracker.AddIngredient(IngredientList.ingredients[2]);
                shaker.UseIngredient(IngredientList.ingredients[2]);
                break;
            case "Ice_cubes":
                tracker.AddIngredient(IngredientList.ingredients[3]);
                shaker.UseIngredient(IngredientList.ingredients[3]);
                break;
            case "Lime_slices":
                tracker.AddIngredient(IngredientList.ingredients[4]);
                shaker.UseIngredient(IngredientList.ingredients[4]);
                break;
            case "Orange_slices":
                tracker.AddIngredient(IngredientList.ingredients[5]);
                shaker.UseIngredient(IngredientList.ingredients[5]);
                break;
            default:
                Debug.Log(name);
                break;
        }
    }

    #endregion

    private void onFailure(string error)
    {
        Debug.Log(error);
    }

}
