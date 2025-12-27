using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "ItemRecipe", menuName = "Scriptable Object/ItemRecipe")]
public class ItemRecipe : ScriptableObject
{
    public int item;
    public string itemName;

    [Header("제작 재료")]
    public int ingredients_1;
    public int ingredients_2;
    public int ingredients_3;
    public int ingredients_4;

}
