using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRenderer : MonoBehaviour
{
    [SerializeField] private List<GameObject> monsters = new List<GameObject>();
    private GameObject currentMonster;
    private Monster monster;

    public void SetRandomMonster()
    {
        currentMonster = Instantiate(monsters[Random.Range(0, monsters.Count)], transform);
        monster = currentMonster.GetComponent<Monster>();
    }

    public void SetMonster(int m)
    {
        currentMonster = Instantiate(monsters[m], transform);
        monster = currentMonster.GetComponent<Monster>();
    }

    public void SetMonsterStage(int stage)
    {
        monster.SetStage(stage);
    }

}
