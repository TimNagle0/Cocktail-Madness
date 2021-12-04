using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomerManager : MonoBehaviour
{
    [Header("Customer settings")]
    public float customerSpeed;

    [Header("Needed components")]
    public OrderManager orderManager;
    public GameObject customerPrefab;
    public Transform initialSpawn;


    private float orderTime;
    private float orderVariance;

    public event Action missedOrder;
    
    // A class for storing the orderlocations and keeping track of customers / free spots
    public class OrderLocation
    {
        public Transform orderObject;
        public Vector3 orderLocation;
        public bool isFree;
        public GameObject currentCustomer;

        public OrderLocation(Transform o, bool t)
        {
            orderObject = o;
            orderLocation = orderObject.position;
            isFree = t;
            currentCustomer = null;
        }

    }

    //Orderlocations are the spots where customers stand
    //servinglocations are the colliders where players drag the shaker
    public List<OrderLocation> orderLocations = new List<OrderLocation>();
    public List<BoxCollider2D> servingLocations = new List<BoxCollider2D>();

    

    public void SetCustomerSettings(float ot, float ov)
    {
        orderTime = ot;
        orderVariance = ov;
        AddOrderLocations();
    }

    // Get all the orderlocations by looking for child game objects with boxcolliders
    // Then instantiate them as OrderLocation objects and add them to the list
    // Also get the serving colliders from the objects and add them to their respective list
    private void AddOrderLocations()
    {
        BoxCollider[] ol = GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider bc in ol)
        {
            OrderLocation newLocation = new OrderLocation(bc.transform, true);
            orderLocations.Add(newLocation);
            servingLocations.Add(bc.GetComponentInChildren<BoxCollider2D>());
        }
    }

    public void SpawnTutorialCustomer()
    {
        OrderLocation orderLoc = GetOrderLocation();
        if (orderLoc == null)
            return;
        Vector3 spawnLocation = initialSpawn.position;
        Vector3 targetLocation = orderLoc.orderLocation;
        GameObject newCustomer = Instantiate(customerPrefab, spawnLocation, Quaternion.identity);
        orderLoc.currentCustomer = newCustomer;
        newCustomer.transform.parent = transform;
        CustomerBehaviour cb = newCustomer.GetComponent<CustomerBehaviour>();
        cb.TutorialCustomerSetup(orderLoc.orderObject);
        cb.MoveCustomer(targetLocation, customerSpeed);
    }


    // Set up the coroutine for repeatedly trying to spawn new customers after a set amount of time
    public void SpawnCustomers(float interval)
    {
        InvokeRepeating("CreateNewCustomer", 0f, interval);
    }


    // Returns the first free OrderLocation in the list (from left to right), otherwise returns null
    private OrderLocation GetOrderLocation()
    {
        foreach(OrderLocation loc in orderLocations)
        {
            if (loc.isFree)
            {
                loc.isFree = false;
                return loc;
            }
        }
        return null;
    }


    
    void CreateNewCustomer()
    {
        // Ask for a free location, if there is one, spawn a customer
        OrderLocation orderLoc = GetOrderLocation();
        if (orderLoc == null)
            return;
        Vector3 targetLocation = orderLoc.orderLocation;
        Vector3 spawnLocation = initialSpawn.position;
        GameObject newCustomer = Instantiate(customerPrefab,spawnLocation,Quaternion.identity);

        // Set the new customer as the customer present at the orderlocation
        orderLoc.currentCustomer = newCustomer;
        newCustomer.transform.parent = transform;


        // Set the customer as the customer in an
        CustomerBehaviour cb = newCustomer.GetComponent<CustomerBehaviour>();
        cb.CustomerSetup(orderLoc.orderObject,orderTime,orderVariance);
        cb.MoveCustomer(targetLocation, customerSpeed);
        cb.OnMissedOrder += MissedOrder;
    }

    public void MissedOrder(GameObject customer)
    {
        missedOrder();
        orderManager.FailCustomer(customer);
    }


    // Remove the customer from the orderlocation and destroy the gameobject
    public void RemoveCustomer(GameObject customer)
    {
        foreach(OrderLocation o in orderLocations)
        {
            if(o.currentCustomer == customer)
            {
                o.currentCustomer = null;
                o.isFree = true;
            }
        }        
        Destroy(customer);

    }

}
