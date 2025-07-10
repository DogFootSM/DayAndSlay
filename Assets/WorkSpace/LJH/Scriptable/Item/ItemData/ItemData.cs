using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "itemData", menuName = "Scriptable Object / itemData")]
public class ItemData : ScriptableObject
{
    public Sprite ItemImage;

    public int ItemId;
    public bool IsEquipment;
    public bool IsOverlaped;

    public Parts Parts;
    public WeaponType WeaponType;
    public SubWeaponType SubWeaponType;
    public ItemSet ItemSet;

    public string Name;
    public int Tier;

    public int Strength;
    public int Agility;
    public int Intelligence;

    public float Critical;


    public int Hp;
    public int Attack;
    public int Defence;

    public int BuyPrice;
    public int SellPrice;

    public string ItemDescA;
    public string ItemDescB;

    public List<ItemData> Ingrediants = new();

    
    

}
