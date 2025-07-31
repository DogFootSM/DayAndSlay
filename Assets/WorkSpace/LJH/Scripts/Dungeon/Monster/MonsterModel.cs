using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterModel : MonoBehaviour
{
    MonsterData monsterData;

    public int Id;
    public string Name;
    public int MaxHp;
    public int Hp;
    public int Attack;
    public float AttackRange;
    public float ChaseRange;
    public float MoveSpeed;
    public float AttackCooldown;


    private void Start()
    {
        if (CompareTag("Boss"))
        {
            monsterData = GetComponent<BossMonsterAI>().GetMonsterData();
        }
        else
        {
            monsterData = GetComponent<GeneralMonsterAI>().GetMonsterData();
        }

        Id = monsterData.Id;
        Name = monsterData.Name;
        MaxHp = monsterData.Hp;
        Hp = monsterData.Hp;
        Attack = monsterData.Attack;
        AttackRange = monsterData.AttackRange;
        ChaseRange = monsterData.ChaseRange;
        MoveSpeed = monsterData.MoveSpeed;
        AttackCooldown = monsterData.AttackCooldown;

    }
    public int GetMonsterMaxHp() => MaxHp;

    public int GetMonsterHp() => Hp;

    public void SetMonsterHp(int hp)
    {
        Hp += hp;
    }
}
