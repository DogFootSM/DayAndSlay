using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Monster Data", menuName = "Scriptable Object/Monster Data")]
public class MonsterData : ScriptableObject
{
    public int hp;
    public int attack;
    public int defense;

    //추후 아이템 클래스 생성시 그것으로 교체
    public List<GameObject> dropTable;
}
