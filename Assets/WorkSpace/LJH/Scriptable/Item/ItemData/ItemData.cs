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
    public float Range;

    public int BuyPrice;
    public int SellPrice;

    public string ItemDescA;
    public string ItemDescB;
    
    public int ingredients_1;
    public int ingredients_2;
    public int ingredients_3;
    public int ingredients_4;

    public List<int> Ingrediants = new();


    
    

}
