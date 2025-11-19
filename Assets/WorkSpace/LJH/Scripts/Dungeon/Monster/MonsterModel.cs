using System.Collections.Generic;
using UnityEngine;

public class MonsterModel : MonoBehaviour
{
    MonsterData monsterData;
    MonsterView monsterView;

    public int Id;
    public string Name;
    public float MaxHp;
    
    public float curHp;
    public float CurHp
    {
        get => curHp;
        set
        {
            curHp = value; 
            monsterView.OnChangeHealth?.Invoke(curHp / MaxHp);
        }
    }
    
    
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
            monsterData = GetComponent<MonsterAI>().GetMonsterData();
        }

        Id = monsterData.Id;
        Name = monsterData.Name;
        MaxHp = monsterData.Hp;
        curHp = monsterData.Hp;
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
        
        monsterView = GetComponent<MonsterView>();
    }

    public ItemData DropItemPick(float randomNum)
    {
        float firstItemRate = dropItemEntries[0].DropRate;
        float secondItemRate = firstItemRate + dropItemEntries[1].DropRate;
        float thirdItemRate = secondItemRate + dropItemEntries[2].DropRate;
        
        
        //ex 1 ~ 10
        if (randomNum <= firstItemRate)
        {
            return ItemDatabaseManager.instance.GetItemByID(dropItemEntries[0].ItemId);
        }
        
        //ex 11 ~ 40
        else if (randomNum > firstItemRate && randomNum <= secondItemRate)
        {
            return ItemDatabaseManager.instance.GetItemByID(dropItemEntries[1].ItemId);
        }
        
        //ex 41 ~ 70
        else if (randomNum > secondItemRate && randomNum <= thirdItemRate)
        {
            return ItemDatabaseManager.instance.GetItemByID(dropItemEntries[2].ItemId);
        }

        //나머지의 경우 드랍템 없음 처리
        return null;

    }
    public float GetMonsterMaxHp() => MaxHp;

    public float GetMonsterHp() => curHp;

    public void SetMonsterHp(float hp)
    {
        CurHp += hp;
    }
}
