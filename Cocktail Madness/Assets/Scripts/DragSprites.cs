using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragSprites : MonoBehaviour
{
    private Vector3 startPosition;
    private Collider2D col;


    void Start()
    {
        col = GetComponent<Collider2D>();
        startPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        if (!PauseControl.gameIsPaused)
        {
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(cursorPos.x, cursorPos.y, 0f);
        }        
    }

    private void OnMouseUp()
    {
        if (!PauseControl.gameIsPaused)
        {
            ResetIngredientSprite();
        }
        
    }

    private void ResetIngredientSprite()
    {
        transform.position = startPosition;
    }


}
