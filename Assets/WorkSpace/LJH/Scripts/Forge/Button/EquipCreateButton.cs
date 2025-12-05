using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipCreateButton : MonoBehaviour
{
    [SerializeField] ItemCreatePopup createPopup;
    public InventoryInteraction inventory;
    public ItemData curSelectedItem {get;  set;}
    
    [SerializeField] private List<InventorySlot> slotList = new List<InventorySlot>();
    
    private void Start()
    {
        inventory = GameObject.FindWithTag("Player").GetComponentInChildren<InventoryInteraction>();
        GetComponent<Button>().onClick.AddListener(CreateItem);
        
    }
    
    /// <summary>
    /// 아이템 버튼에서 호출// 선택한 아이템 등록해주는 메서드
    /// </summary>
    /// <param name="item"></param>
    public void SetCurSelectedItem(ItemData item) =>  curSelectedItem = item;

    public void CreateItem()
    {
        slotList[0]?.RemoveItem(curSelectedItem.ingredients_1_Count);
        slotList[1]?.RemoveItem(curSelectedItem.ingredients_2_Count);
        slotList[2]?.RemoveItem(curSelectedItem.ingredients_3_Count);
        slotList[3]?.RemoveItem(curSelectedItem.ingredients_4_Count);
        
        StartCoroutine(CreateCoroutine());
    }

    private IEnumerator CreateCoroutine()
    {
        createPopup.gameObject.SetActive(true);
        yield return new WaitUntil( () => !createPopup.gameObject.activeSelf);
        
        inventory.AddItemToInventory(curSelectedItem);
    }
    
    public void SetSlotList(List<InventorySlot> slotList) => this.slotList = slotList;
}
