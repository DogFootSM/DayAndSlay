using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Monster Data", menuName = "Scriptable Object/Monster Data")]
public class MonsterData : ScriptableObject
{
    public string monsterName;
    public int id;
    public int hp;
    public int attack;
    public int range;
    public float cooldown;

    //추후 아이템 클래스 생성시 그것으로 교체
    public List<ItemData> dropTable;
}
