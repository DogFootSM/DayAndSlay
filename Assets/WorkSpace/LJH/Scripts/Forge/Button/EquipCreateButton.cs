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
    }
}
