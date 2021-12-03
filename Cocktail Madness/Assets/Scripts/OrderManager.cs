using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{

    public CustomerManager customerManager;
    public UIManager uiManager;
    public Shaker shaker;
    private AudioSource audioSource;

    [SerializeField] private AudioClip correctOrderSound;
    [SerializeField] private AudioClip incorrectOrderSound;

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
    public void ServeOrder()
    {
        CompareOrders();
    }

    private void CompareOrders()
    {
        Recipe preparedOrder = shaker.CreateOrder();

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
            FailCustomer(null);

        }
        else
        {
            Debug.Log("ingredients");
            uiManager.UpdateServingMessage(false, "ingredients");
            FailCustomer(null);
        }
    }
    #endregion

    public void FailCustomer(GameObject customer)
    {
        
        PlayerStats.lives --;
        PlayerStats.incorrectServings++;
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
        audioSource.PlayOneShot(correctOrderSound);
        
        if (isPerfect)
        {
            PlayerStats.perfectServings++;
            uiManager.UpdatePerfectServed(PlayerStats.perfectServings);
        }
        else
        {
            PlayerStats.correctServings++;
            uiManager.UpdateServed(PlayerStats.correctServings);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CompareOrders();

        } 
    }
}