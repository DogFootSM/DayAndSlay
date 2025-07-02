using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterModel : MonoBehaviour
{
    MonsterData monsterData;

    public int Id;
    public string Name;
    public int Hp;
    public int Attack;
    public float AttackRange;
    public float ChaseRange;
    public float MoveSpeed;
    public float AttackCooldown;


    private void Start()
    {
        monsterData = GetComponent<GeneralMonsterAI>().monsterData;

        Id = monsterData.Id;
        Name = monsterData.Name;
        Hp = monsterData.Hp;
        Attack = monsterData.Attack;
        AttackRange = monsterData.AttackRange;
        ChaseRange = monsterData.ChaseRange;
        MoveSpeed = monsterData.MoveSpeed;
        AttackCooldown = monsterData.AttackCooldown;

    }
}
