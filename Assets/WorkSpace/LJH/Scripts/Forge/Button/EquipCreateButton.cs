using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipCreateButton : MonoBehaviour
{
    public InventoryInteraction inventory;
    public ItemData curSelectedItem {get;  set;}
    private void Start()
    {
        inventory = GameObject.FindWithTag("Player").GetComponentInChildren<InventoryInteraction>();
        
    }
    
    /// <summary>
    /// 아이템 버튼에서 호출// 선택한 아이템 등록해주는 메서드
    /// </summary>
    /// <param name="item"></param>
    public void SetCurSelectedItem(ItemData item) =>  curSelectedItem = item;

    public void CreateItem()
    {
        inventory.AddItemToInventory(curSelectedItem);
        inventory.RemoveItemFromInventory(curSelectedItem.ingredients_1, curSelectedItem.ingredients_1_Count);
        inventory.RemoveItemFromInventory(curSelectedItem.ingredients_2, curSelectedItem.ingredients_2_Count);
        inventory.RemoveItemFromInventory(curSelectedItem.ingredients_3, curSelectedItem.ingredients_3_Count);
        inventory.RemoveItemFromInventory(curSelectedItem.ingredients_4, curSelectedItem.ingredients_4_Count);
    }
}
