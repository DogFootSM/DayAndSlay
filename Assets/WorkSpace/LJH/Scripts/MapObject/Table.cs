using TMPro;
using UnityEngine;
using Zenject;

public class Table : InteractableObj
{

    [SerializeField] private SpriteRenderer registeredItemRenderer;

    [Inject(Id = "PopUp")] private GameObject popUp;
    //private PopUp tableAskPopup;
    private TextMeshProUGUI tableAskText;
    [SerializeField] private ItemData curItemData;
    public ItemData CurItemData
    {
        get => curItemData;
        set => curItemData = value;
    }
    public override void Interaction()
    {
    }

    public override void UiOnOffMethod(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        
        string _objName;
          
        _objName = popUp.GetComponent<PopUp>().objName = "가판대";
        

        if (curItemData != null)
        {
            popUp.GetComponent<PopUp>().SetText($"{_objName}에서 아이템을 회수하시겠습니까?");
        }
        else
        {
            popUp.GetComponent<PopUp>().SetText($"스페이스바를 눌러서 {_objName}에 아이템을 등록하세요.");
        }

        popUp.SetActive(!popUp.gameObject.activeSelf);
    }


    /// <summary>
    /// 플레이어 인벤토리로부터 아이템 데이터를 받아옴
    /// </summary>
    /// <param name="item">인벤토리에서 넘겨 받은 아이템 데이터</param>
    public void TakeItem(ItemData item)
    {
        curItemData = item;
        registeredItemRenderer.GetComponent<SpriteRenderer>().sprite = item.ItemImage;
    }

    /// <summary>
    /// 테이블에 등록되어 있는 아이템을 반환하여 인벤토리에 다시 추가
    /// </summary>
    /// <param name="inventoryInteraction">플레이어가 가지고 있는 인벤토리</param>
    public void GiveItem(InventoryInteraction inventoryInteraction)
    {
        inventoryInteraction.AddItemToInventory(curItemData);
        curItemData = null;
        registeredItemRenderer.GetComponent<SpriteRenderer>().sprite = null;
    }
}