using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderTimer : MonoBehaviour
{
    [SerializeField] List<Sprite> orderBubbleStates = new List<Sprite>();
    [SerializeField] MonsterRenderer monsterRenderer;
    [SerializeField] Image orderBubble;
    [SerializeField] Animator animator;
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
        StartCoroutine(OrderBubble(orderTime));

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
            PlayHurryAnimation();
        }

    }

    IEnumerator OrderBubble(float totalTime)
    {
        float interval = totalTime / 10;
        int orderSprite = 0;
        while (orderSprite < orderBubbleStates.Count)
        {
            orderBubble.sprite = orderBubbleStates[orderSprite];
            orderSprite++;
            yield return new WaitForSeconds(interval);
        }
        
    }

    private void PlayHurryAnimation()
    {
        animator.SetBool("isLate", true);
    }
    // Set the sprites of monsters and order bubbles
    // Monsters are offset by one because their base sprite is also in their array
    private void UpdateOrderSprites(int sprite)
    {
        monsterRenderer.SetMonsterStage(sprite + 1);
        //orderBubble.sprite = orderBubbleStates[sprite];
    }
}
