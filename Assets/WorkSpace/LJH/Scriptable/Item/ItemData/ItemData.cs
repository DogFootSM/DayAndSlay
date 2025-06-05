using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ItemStruct
{
    public bool isWeapon;

    public Parts parts;
    public CharacterWeaponType weaponType;
    public SubWeaponType subWeaponType;

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

    public Parts parts;
    public CharacterWeaponType weaponType;
    public SubWeaponType subWeaponType;


    public string Name;
    public int Tier;
    public int Attack;
    public int Deffence;

    public int BuyPrice;
    public int SellPrice;

    public string ItemDescA;
    public string ItemDescB;

    
    public ItemStruct GetItemData()
    {
        ItemStruct weaponStruct = new ItemStruct();
    
        weaponStruct.isWeapon = isWeapon;

        weaponStruct.parts = parts;
        weaponStruct.weaponType = weaponType; ;
        weaponStruct.subWeaponType = subWeaponType;

        weaponStruct.Name = Name;
        weaponStruct.Tier = Tier;
        weaponStruct.Attack = Attack;
    
        weaponStruct.BuyPrice = BuyPrice;
        weaponStruct.SellPrice = SellPrice;
    
        return weaponStruct;
    }
    

}
