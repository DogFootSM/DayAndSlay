using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "ItemRecipe", menuName = "Scriptable Object/ItemRecipe")]
public class ItemRecipe : ScriptableObject
{
    public string itemName;
    public detailType_Armor armorType;
    public detailType_Weapon weaponType;

    [Header("제작 재료")]
    public Ingredient ingredients_1;
    public Ingredient ingredients_2;
    public Ingredient ingredients_3;
    public Ingredient ingredients_4;
    public Ingredient ingredients_5;

    public bool isOpend;
}
