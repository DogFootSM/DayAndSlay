using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipCreateButton : MonoBehaviour
{
    [SerializeField] ItemCreatePopup createPopup;
    [SerializeField] CreateOkayPopup okayPopup;
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
        
        if(curSelectedItem.ingredients_1_Count != 0) slotList[0]?.RemoveItem(curSelectedItem.ingredients_1_Count);
        if(curSelectedItem.ingredients_2_Count != 0) slotList[1]?.RemoveItem(curSelectedItem.ingredients_2_Count);
        if(curSelectedItem.ingredients_3_Count != 0) slotList[2]?.RemoveItem(curSelectedItem.ingredients_3_Count);
        if(curSelectedItem.ingredients_4_Count != 0) slotList[3]?.RemoveItem(curSelectedItem.ingredients_4_Count);
        
        StartCoroutine(CreateCoroutine());
    }

    private IEnumerator CreateCoroutine()
    {
        //제작 대기 팝업
        createPopup.gameObject.SetActive(true);
        yield return new WaitUntil( () => !createPopup.gameObject.activeSelf);
        
        //제작 완료 팝업
        okayPopup.SetCurItemInfo(curSelectedItem);
        okayPopup.gameObject.SetActive(true);
        yield return new WaitUntil( () => !okayPopup.gameObject.activeSelf);
        
        inventory.AddItemToInventory(curSelectedItem);
    }
    
    public void SetSlotList(List<InventorySlot> slotList) => this.slotList = slotList;
}
