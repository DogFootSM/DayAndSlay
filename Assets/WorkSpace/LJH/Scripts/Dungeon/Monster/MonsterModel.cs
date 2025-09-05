using System.Collections.Generic;
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
    public List<ItemData> dropItems = new List<ItemData>();
    
    private List<DropItemEntry> dropItemEntries = new List<DropItemEntry>();


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
        dropItemEntries = monsterData.DropTable;

        foreach (DropItemEntry entry in dropItemEntries)
        {
            dropItems.Add(ItemDatabaseManager.instance.GetItemByID(entry.ItemId));
        }
    }
    public float GetMonsterMaxHp() => MaxHp;

    public float GetMonsterHp() => Hp;

    public void SetMonsterHp(float hp)
    {
        Hp += hp;
    }
}
