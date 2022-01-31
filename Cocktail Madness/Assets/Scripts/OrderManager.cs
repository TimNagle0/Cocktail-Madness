using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OrderManager : MonoBehaviour
{

    public CustomerManager customerManager;
    public UIManager uiManager;
    public Shaker shaker;
    private AudioSource audioSource;

    private KeyCode servingSpot1 = KeyCode.Alpha1;
    private KeyCode servingSpot2 = KeyCode.Alpha2;
    private KeyCode servingSpot3 = KeyCode.Alpha3;
    private KeyCode servingSpot4 = KeyCode.Alpha4;

    [SerializeField] private AudioClip correctOrderSound;
    [SerializeField] private AudioClip incorrectOrderSound;

    public event Action incorrectOrder;
    public event Action perfectOrder;
    public event Action correctOrder;
    public event Action<float> shakeTime;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        shaker.serveCustomer += ServeOrderLocation;
    }

    #region order comparisons

    public void ServeOrderLocation(BoxCollider2D location)
    {
        CompareOrders(customerManager.servingLocations.IndexOf(location));
    }
    public void ServeOrderLocation(int location)
    {
        CompareOrders(location);
    }
    public void ServeOrder()
    {
        CompareOrders();
    }

    private void CompareOrders()
    {
        Recipe preparedOrder = shaker.CreateOrder();
        shakeTime(preparedOrder.shakeTime);
        foreach(CustomerManager.OrderLocation orderLocation in customerManager.orderLocations)
        {
            if(orderLocation.currentCustomer == null)
            {
                break;
            }
            CustomerBehaviour cb = orderLocation.currentCustomer.GetComponent<CustomerBehaviour>();
            Recipe currentOrder = cb.order;
            
            bool ingredients = CompareIngredientLists.CompareLists(preparedOrder.Ingredients, currentOrder.Ingredients);
            bool shakeTime= CompareIngredientLists.CompareShakeTimes(currentOrder.shakeTime, preparedOrder.shakeTime);
            bool isPerfect = cb.isPerfectOrder();
            if (ingredients && shakeTime)
            {
                OrderResults(ingredients, shakeTime,isPerfect, orderLocation.currentCustomer);
                return;
            }

        }
        CompareOrders(0);
    }
 
    public void CompareOrders(int spot)
    {
        //Get the customer from the current orderlocation
        GameObject customer = customerManager.orderLocations[spot].currentCustomer;
        if(customer == null)
        {
            Debug.Log("there is no customer there");
            return;
        }

        CustomerBehaviour cb = customer.GetComponent<CustomerBehaviour>();
        Recipe preparedOrder = shaker.CreateOrder();
        Recipe currentOrder = cb.order;

        //Get the shaketime and see if it is a perfect order
        bool ingredients = CompareIngredientLists.CompareLists(preparedOrder.Ingredients, currentOrder.Ingredients);
        bool shakeTime = CompareIngredientLists.CompareShakeTimes(currentOrder.shakeTime, preparedOrder.shakeTime);
        bool isPerfect = cb.isPerfectOrder();
        OrderResults(ingredients, shakeTime, isPerfect, customer);
    }

    private void OrderResults(bool ingredients, bool shakeTime, bool isPerfect,GameObject customer)
    {
        Debug.Log("looking at results");
        if (ingredients && shakeTime)
        {
            Debug.Log("correct");
            ServeCustomer(customer,isPerfect);
            uiManager.UpdateServingMessage(true, "correct");

            return;
        }
        else if (ingredients && !shakeTime)
        {
            Debug.Log("shaketime");
            uiManager.UpdateServingMessage(false, "shaketime");
            if (!customer.GetComponent<CustomerBehaviour>().isTutorial)
            {
                FailCustomer(null);
            }
            

        }
        else
        {
            Debug.Log("ingredients");
            uiManager.UpdateServingMessage(false, "ingredients");
            if (!customer.GetComponent<CustomerBehaviour>().isTutorial)
            {
                FailCustomer(null);
            }
        }
    }
    #endregion

    public void FailCustomer(GameObject customer)
    {
        
        PlayerStats.lives --;
        PlayerStats.incorrectServings++;
        incorrectOrder();
        if (PlayerStats.lives == 0)
        {
            uiManager.TakeDamage();
            GameOver();
        }
        uiManager.TakeDamage();

        if(customer != null)
        {
            customerManager.RemoveCustomer(customer);
        }
    }

    public void GameOver()
    {
        uiManager.gameOver.ShowGameOverScreen();
    
    }

    void ServeCustomer(GameObject customer,bool isPerfect)
    {
        
        shaker.ResetShaker();
        customerManager.RemoveCustomer(customer);
        uiManager.GainPoint();
        
        if (isPerfect)
        {
            PlayerStats.perfectServings++;
            perfectOrder();
            uiManager.UpdatePerfectServed(PlayerStats.perfectServings);
        }
        else
        {
            PlayerStats.correctServings++;
            correctOrder();
            uiManager.UpdateServed(PlayerStats.correctServings);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(servingSpot1))
        {
            CompareOrders(0);
        }
        if (Input.GetKeyDown(servingSpot2))
        {
            CompareOrders(1);
        }
        if (Input.GetKeyDown(servingSpot3))
        {
            CompareOrders(2);
        }
        if (Input.GetKeyDown(servingSpot4))
        {
            CompareOrders(3);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CompareOrders();

        } 
    }
}
