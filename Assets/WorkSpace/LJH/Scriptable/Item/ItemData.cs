using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public struct ItemStruct
{
    public bool isWeapon;
    public detailType_Weapon DetailType_Weapon;
    public detailType_Armor DetailType_Armor;

    public string Name;
    public int Tier;
    public int Attack;
    public int Deffence;

    public int BuyPrice;
    public int SellPrice;
}
[CreateAssetMenu (fileName = "itemData", menuName = "Scriptable Object / itemData")]
public class ItemData : ScriptableObject
{
    [Header("아이템 데이터 분류값")]
    public int ItemId;
    public bool isWeapon;
    public detailType_Weapon Detail_Weapon;
    public detailType_Armor Detail_Armor;

    [Header("아이템 정보")]
    public string Name;
    public int Tier;
    public int Attack;
    public int Deffence;

    public int BuyPrice;
    public int SellPrice;


    public ItemStruct GetItemData(ItemStruct itemStruct)
    {
        return itemStruct = ThisItemIsWeapon(isWeapon);
    }

    ItemStruct ThisItemIsWeapon(bool isWeapon)
    {
        if(!isWeapon)
        {
            return GetArmorData();
        }
    
        return GetWeaponData();
    }
    
    ItemStruct GetWeaponData()
    {
        ItemStruct weaponStruct = new ItemStruct();
    
        weaponStruct.isWeapon = isWeapon;
        weaponStruct.DetailType_Weapon = Detail_Weapon;
        weaponStruct.DetailType_Armor = detailType_Armor.NotArmor;

        weaponStruct.Name = Name;
        weaponStruct.Tier = Tier;
        weaponStruct.Attack = Attack;
    
        weaponStruct.BuyPrice = BuyPrice;
        weaponStruct.SellPrice = SellPrice;
    
        return weaponStruct;
    }
    
    ItemStruct GetArmorData()
    {
        ItemStruct armorStruct = new ItemStruct();

        armorStruct.isWeapon = isWeapon;
        armorStruct.DetailType_Weapon = detailType_Weapon.NotWeapon;
        armorStruct.DetailType_Armor = Detail_Armor;

        armorStruct.Name = Name;
        armorStruct.Tier = Tier;
        armorStruct.Deffence = Deffence;
    
        armorStruct.BuyPrice = BuyPrice;
        armorStruct.SellPrice = SellPrice;
    
        return armorStruct;
    }

}
