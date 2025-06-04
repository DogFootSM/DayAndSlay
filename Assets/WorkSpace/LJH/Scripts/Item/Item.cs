using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemData itemData;

    [Header("아이템 정보 필드")]
    public int ItemId;
    public bool IsWeapon;

    public Parts Parts;
    public WeaponType WeaponType;
    public SubWeaponType SubWeaponType;

    [Header("아이템 인게임 정보 필드")]
    public string Name;
    public int Tier;
    public int Attack;
    public int Deffence;

    public int BuyPrice;
    public int SellPrice;

    ItemStruct itemStruct;

    private void Start()
    {
        Init();
    }

    void Init()
    { 
        itemData = BSM_ItemManager.ItemManager.instance.GetItemData(ItemId);
        itemStruct = itemData.GetItemData();

        IsWeapon = itemStruct.isWeapon;
        Parts = itemStruct.parts;
        WeaponType = itemStruct.weaponType; ;
        SubWeaponType = itemStruct.subWeaponType;

        Name = itemStruct.Name;
        Tier = itemStruct.Tier;
        Attack = itemStruct.Attack;
        Deffence = itemStruct.Deffence;
        
        BuyPrice = itemStruct.BuyPrice;
        SellPrice = itemStruct.SellPrice;
        
        gameObject.name = Name; 
    }
}
