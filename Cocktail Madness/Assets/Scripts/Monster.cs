using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public List<Sprite> stages = new List<Sprite>();
    public SpriteRenderer sr;

    private void Awake()
    {
        sr.sprite = stages[0];
    }

    public void SetStage(int stage)
    {
        sr.sprite = stages[stage];
    }


}
