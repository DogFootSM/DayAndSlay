using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ItemStruct
{
    public bool isWeapon;
    public Type_Weapon DetailType_Weapon;
    public Type_Armor DetailType_Armor;

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
    public Sprite ItemImage;

    public int ItemId;
    //Todo : isWeapon을 isEquipment로 변경해야함
    public bool isWeapon;
    public bool IsOverlaped;
    public Type_Weapon Detail_Weapon;
    public Type_Armor Detail_Armor;

    public string Name;
    public int Tier;
    public int Attack;
    public int Deffence;

    public int BuyPrice;
    public int SellPrice;

    public string ItemDescA;
    public string ItemDescB;

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
        weaponStruct.DetailType_Armor = Type_Armor.NotArmor;

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
        armorStruct.DetailType_Weapon = Type_Weapon.NotWeapon;
        armorStruct.DetailType_Armor = Detail_Armor;

        armorStruct.Name = Name;
        armorStruct.Tier = Tier;
        armorStruct.Deffence = Deffence;
    
        armorStruct.BuyPrice = BuyPrice;
        armorStruct.SellPrice = SellPrice;
    
        return armorStruct;
    }

}
