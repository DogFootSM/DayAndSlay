using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemData itemData;

    [Header("아이템 정보 필드")]
    public int ItemId;
    public bool isWeapon;
    public Type_Weapon detail_Weapon;
    public Type_Armor detail_Armor;

    [Header("아이템 인게임 정보 필드")]
    public string Name;
    public int Tier;
    public int attack;
    public int deffence;

    public int buyPrice;
    public int sellPrice;

    ItemStruct itemStruct;

    private void Start()
    {
        Init();
    }

    void Init()
    { 
        itemData = BSM_ItemManager.ItemManager.instance.GetItemData(ItemId);
        itemStruct = itemData.GetItemData(itemStruct);

        isWeapon = itemStruct.isWeapon;
        detail_Weapon = itemStruct.DetailType_Weapon;
        detail_Armor = itemStruct.DetailType_Armor;
        
        Name = itemStruct.Name;
        Tier = itemStruct.Tier;
        attack = itemStruct.Attack;
        deffence = itemStruct.Deffence;
        
        buyPrice = itemStruct.BuyPrice;
        sellPrice = itemStruct.SellPrice;
        
        gameObject.name = Name; 
    }
}
