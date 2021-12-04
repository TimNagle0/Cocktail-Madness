using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CustomerBehaviour : MonoBehaviour
{
    public Recipe order;
    public GameObject orderBubble;
    public Image orderRenderer;

    public event Action<GameObject> OnMissedOrder;

    public OrderTimer orderTimer;

    [Header("Tutorial monster settings")]
    public int monsterType;
    public int recipe;


    private float orderTime;
    private float variance;
    private float perfectTime;
    private float startTime = Mathf.Infinity;
    private bool isMoving = false;
    private bool isTutorial = false;

    private Transform orderLocation;


    //Setting up the normal customers
    public void CustomerSetup(Transform ol, float t,float v)
    {
        orderLocation = ol;
        orderTime = t;
        variance = v;
        GetComponentInChildren<MonsterRenderer>().SetRandomMonster();
    }

    #region Tutorial Setup
    //Setting up the tutorial customer
    public void TutorialCustomerSetup(Transform ol)
    {
        orderLocation = ol;
        orderTime = Mathf.Infinity;
        isTutorial = true;
        GetComponentInChildren<MonsterRenderer>().SetMonster(monsterType);
    }

    //Start a tutorial order without timer and a simple recipe
    public void StartTutorialOrder()
    {
        orderBubble.SetActive(true);
        order = RecipeList.recipeList[0];
        orderRenderer.sprite = order.orderSprite;
        orderRenderer.SetNativeSize();
        orderRenderer.transform.localScale *= 1.5f;
    }

    #endregion


    //Starts the timer for the order and sets a recipe for the customer
    public void StartCustomerOrder(float time)
    {
        orderBubble.SetActive(true);
        orderTime = time;
        perfectTime = orderTime * 0.5f;
        startTime = Time.time;
        GetRecipe();
        orderTimer.SetupTimer(orderTime);

    }
    //Gets a random recipe from the list and displays the icon in the order bubble
    private void GetRecipe()
    {
        order = RecipeList.recipeList[UnityEngine.Random.Range(0, 3)];
        orderRenderer.sprite = order.orderSprite;
        orderRenderer.SetNativeSize();
        orderRenderer.transform.localScale *= 1.3f;
    }

    public bool isPerfectOrder()
    {
        Debug.Log("Perfect time = " + perfectTime);
        bool isPerfect = Time.time - startTime < perfectTime;
        Debug.Log("is perfect order : " + isPerfect);
        return isPerfect;
    }


    //Updates the timer for the order and calls an event when the timer has run out.
    private void Update()
    {
        if (isTutorial)
            return;
        if(Time.time - startTime > orderTime)
        {
            OnMissedOrder(gameObject);
        }
        
    }

    //Function to call the movement coroutine from different scripts
    public void MoveCustomer(Vector3 targetLocation, float speed)
    {
        IEnumerator m = MoveToLocation(targetLocation, speed);
        StartCoroutine(m);
    }


    //Simple movement coroutine to move the monster to an orderlocation
    public IEnumerator MoveToLocation(Vector3 targetLocation,float speed)
    {
        if (isMoving)
            yield break;
        else
        {
            isMoving = true;
        }
        while(isMoving)
        {
            
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetLocation, step);

            if (Vector3.Distance(transform.position, targetLocation) < 0.1f)
            {
                transform.position = targetLocation;
                isMoving = false;
            }
            yield return new WaitForEndOfFrame();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == orderLocation)
        {
            if (isTutorial)
            {
                StartTutorialOrder();
            }
            else
            {
                StartCustomerOrder(UnityEngine.Random.Range(orderTime - variance, orderTime + variance));
            }
            
        }
    }

}
