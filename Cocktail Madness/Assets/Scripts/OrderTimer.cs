using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderTimer : MonoBehaviour
{
    [SerializeField] List<Sprite> orderBubbleStates = new List<Sprite>();
    [SerializeField] MonsterRenderer monsterRenderer;
    [SerializeField] Image orderBubble;

    private bool isStarted = false;
    private float orderTime;
    private float startTime;

    private enum CustomerState
    {
        happy,
        patient,
        annoyed,
        angry
        
    }
    private CustomerState customerState;
    public void SetupTimer(float time)
    {
        customerState = CustomerState.happy;
        orderTime = time;
        startTime = Time.time;
        isStarted = true;

    }


    // Some logic to determine which images for the customers and order timers to display
    void Update()
    {
        if (!isStarted)
            return;
        if(Time.time - startTime > orderTime * 0.5f && customerState == CustomerState.happy)
        {
            customerState = CustomerState.patient;
            UpdateOrderSprites(0);

        }else if (Time.time - startTime > orderTime * 0.75f && customerState == CustomerState.patient)
        {
            UpdateOrderSprites(1);
            customerState = CustomerState.annoyed;
        }else if (Time.time - startTime > orderTime * 0.875f && customerState == CustomerState.annoyed)
        {
            UpdateOrderSprites(2);
            customerState = CustomerState.angry;
        }

    }
    // Set the sprites of monsters and order bubbles
    // Monsters are offset by one because their base sprite is also in their array
    private void UpdateOrderSprites(int sprite)
    {
        monsterRenderer.SetMonsterStage(sprite + 1);
        orderBubble.sprite = orderBubbleStates[sprite];
    }
}
