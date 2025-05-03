using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ItemData itemData;

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

    private void Start()
    {
        ItemId  = itemData.ItemId;
        Type = itemData.Type;
        detail_Weapon = itemData.detail_Weapon;
        detail_Armor = itemData.detail_Armor;

        name = itemData.Name;
        Tier = itemData.Tier;
        attack = itemData.attack;
        deffence = itemData.deffence;

        buyPrice = itemData.buyPrice;
        sellPrice = itemData.sellPrice;
    }
}
