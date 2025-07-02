using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class Table : InteractableObj
{
    //아이템 보유 여부
    private bool isHave;

    public ItemData item;

    private Sprite itemImage;
    private SpriteRenderer tableItem;
    //임시용
    private InventoryInteraction inventory;
    [Inject(Id = "PopUp")]
    GameObject popUp;

    // 테이블의 내용물을 저장하기 위한 마스터 클래스가 필요함

    void Start()
    {
        tableItem = transform.GetChild(0).GetComponent<SpriteRenderer>();
        isHave = item ? false : true;
    }



    public override void Interaction(ItemData item)
    {
        Debug.Log("테이블 사용");
        //Todo: 테이블 사용


        if (isHave)
        {
            Debug.Log("아이템을 뺐습니다.");
            GiveItem();
        }

        else
        {
            Debug.Log("아이템을 넣었습니다.");
            //TakeItem(item);
        }
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

    /// <summary>
    /// 테이블에 아이템 넣어주는 테스트 코드
    /// </summary>
    /// <param name="item"></param>
    private void SettingItem(ItemData item)
    {
        this.item = item;
        itemImage = item.ItemImage;
        tableItem.sprite = itemImage;
    }


}