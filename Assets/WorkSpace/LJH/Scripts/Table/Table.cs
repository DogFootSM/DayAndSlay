using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class Table : InteractableObj, IInteractionStoreScene
{
    //아이템 보유 여부
    private bool isHave;

    [SerializeField] public ItemData item;
    Sprite itemImage;

    SpriteRenderer tableItem;
    //임시용
    InventoryInteraction inventory;
    [Inject(Id = "PopUp")]
    GameObject popUp;

    void Start()
    {
        tableItem = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (item == null)
        {
            isHave = false;
        }
        else
        {
            isHave = true;
        }
    }

    public override void Interaction()
    {
        Debug.Log("테이블 사용");
        //Todo: 테이블 사용

        //if(isHave)
        //{
        //    GiveItem();
        //}
        //
        //else
        //{
            //}
    }

    public override void UiOnOffMethod(Collision2D collision)
    {
        popUp.GetComponent<PopUp>().objName = "가판대";
        popUp.SetActive(!popUp.gameObject.activeSelf);
    }


    /// <summary>
    /// 플레이어가 테이블에 아이템 집어넣을 때
    /// </summary>
    /// <param name="item"></param>
    public void TakeItem(ItemData item)
    {
        this.item = item;
        itemImage = item.GetComponent<Item>().itemData.ItemImage;
        tableItem.sprite = itemImage;
        isHave = true;

    }

    /// <summary>
    /// 플레이어가 테이블에서 아이템 꺼낼 때
    /// </summary>
    public void GiveItem()
    {
        ItemData item = this.item;
        this.item = null;
        itemImage = null;
        tableItem.sprite = null;

        inventory.AddItemToInventory(item);
        isHave = false;
    }

    private void SettingItem(ItemData item)
    {
        this.item = item;
        itemImage = item.ItemImage;
        tableItem.sprite = itemImage;
    }


}
