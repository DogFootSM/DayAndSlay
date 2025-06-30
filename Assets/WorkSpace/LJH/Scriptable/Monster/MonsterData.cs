using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[System.Serializable]
public class DropItemEntry
{
    public int ItemId;
    public float DropRate; // 0~100 Сп ШЎЗќ
}

[CreateAssetMenu (fileName = "Monster Data", menuName = "Scriptable Object/Monster Data")]
public class MonsterData : ScriptableObject
{
    public int Id;
    public string Name;
    public int Hp;
    public int Attack;
    public float AttackRange;
    public float ChaseRange;
    public float MoveSpeed;
    public float AttackCooldown;

    public List<DropItemEntry> DropTable = new();
}
