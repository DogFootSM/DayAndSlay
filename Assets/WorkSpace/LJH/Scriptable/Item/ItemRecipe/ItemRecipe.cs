using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "ItemRecipe", menuName = "Scriptable Object/ItemRecipe")]
public class ItemRecipe : ScriptableObject
{
    public string itemName;

    [Header("제작 재료")]
    public Ingredient ingredients_1;
    public Ingredient ingredients_2;
    public Ingredient ingredients_3;
    public Ingredient ingredients_4;
    public Ingredient ingredients_5;

    public bool isOpend;

    //Todo: 캐릭터 > 인벤토리에 재료 아이템 중 하나가 감지되면 해당 레시피 오픈

    public void FindRecipe()
    {
        isOpend = true;
    }
}
