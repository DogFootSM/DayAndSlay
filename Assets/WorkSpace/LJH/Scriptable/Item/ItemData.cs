using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum itemType
{
    Weapon,
    Armor
}
public enum detailType_Weapon
{
    NotWeapon,
    Sword,
    Axe,
    Spear,
    Wand,
    Dagger
}
public enum detailType_Armor
{
    NotArmor,
    helmet,
    armor,
    arm,
    pants,
    shoes
}
[CreateAssetMenu (fileName = "itemData", menuName = "Scriptable Object / itemData")]
public class ItemData : ScriptableObject
{
    [Header("아이템 데이터 분류값")]
    public int ItemId;
    public itemType Type;
    public detailType_Weapon detail_Weapon;
    public detailType_Armor detail_Armor;

    [Header("아이템 정보")]
    public string Name;
    public int Tier;
    public int attack;
    public int deffence;

    public int buyPrice;
    public int sellPrice;
}
