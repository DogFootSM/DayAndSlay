using UnityEngine;

public class MonsterModel : MonoBehaviour
{
    MonsterData monsterData;

    public int Id;
    public string Name;
    public float MaxHp;
    public float Hp;
    public float def;
    public float Attack;
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
            monsterData = GetComponent<NewMonsterAI>().GetMonsterData();
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
    public float GetMonsterMaxHp() => MaxHp;

    public float GetMonsterHp() => Hp;

    public void SetMonsterHp(float hp)
    {
        Hp += hp;
    }
}
