using TMPro;
using UnityEngine;
using Zenject;

public class Table : InteractableObj
{

    [SerializeField] private SpriteRenderer registeredItemRenderer;

    [Inject(Id = "PopUp")] private GameObject popUp;
    private PopUp tableAskPopup;
    private TextMeshProUGUI tableAskText;
    public ItemData curItemData;
    public ItemData CurItemDataData => curItemData;
    
    public override void Interaction()
    {
    }

    public override void UiOnOffMethod(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("NPC"))
        {
            Npc npc= collision.gameObject.GetComponent<Npc>();
            npc.BuyItemFromTable();

            return;
        }

        if (tableAskPopup == null)
        {
            //TODO: POPUP TEXT 구조 수정되면 GetComponent로 받아오는건 안해도 될듯
            tableAskPopup = popUp.GetComponent<PopUp>();
            tableAskText = popUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            tableAskPopup.objName = "가판대";
        }

        //TODO: 안내 멘트는 수정해야함. 
        if (curItemData != null)
        {
            tableAskText.text = $"{tableAskPopup.objName}에서 아이템을 회수하시겠습니까?";
        }
        else
        {
            tableAskText.text = $"E키를 눌러서 {tableAskPopup.objName}에 아이템을 등록하세요.";
        }

        popUp.SetActive(!popUp.gameObject.activeSelf);
    }


    /// <summary>
    /// 플레이어 인벤토리로부터 아이템 데이터를 받아옴
    /// </summary>
    /// <param name="item">인벤토리에서 넘겨 받은 아이템 데이터</param>
    public void TakeItem(ItemData item)
    {
        this.curItemData = item;
        registeredItemRenderer.sprite = item.ItemImage;
    }

    /// <summary>
    /// 테이블에 등록되어 있는 아이템을 반환하여 인벤토리에 다시 추가
    /// </summary>
    /// <param name="inventoryInteraction">플레이어가 가지고 있는 인벤토리</param>
    public void GiveItem(InventoryInteraction inventoryInteraction)
    {
        inventoryInteraction.AddItemToInventory(curItemData);
        curItemData = null;
        registeredItemRenderer.sprite = null;
    }
}