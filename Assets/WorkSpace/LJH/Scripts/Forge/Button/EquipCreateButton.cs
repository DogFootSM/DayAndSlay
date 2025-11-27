using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipCreateButton : MonoBehaviour
{
    InventoryInteraction inventory;
    public ItemData curSelectedItem {get;  set;}
    private void Start()
    {
		//ToDo : 제작 가져오기
    }
    
    /// <summary>
    /// 아이템 버튼에서 호출// 선택한 아이템 등록해주는 메서드
    /// </summary>
    /// <param name="item"></param>
    public void SetCurSelectedItem(ItemData item) =>  curSelectedItem = item;

    public void CreateItem()
    {
        //Todo : 재료 체크 필요함 
        //예를 들면 curSelectedItem의 Ingrediant가 모두 있지 않으면 버튼 비활성화 or 팝업 노출 같은 식으로
        inventory.AddItemToInventory(curSelectedItem);
    }
}
