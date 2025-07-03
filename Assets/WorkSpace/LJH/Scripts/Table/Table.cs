using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class Table : InteractableObj
{
    //������ ���� ����
    private bool isHave;

    public ItemData item;
    private SpriteRenderer tableItem;
    
    //�ӽÿ�
    private InventoryInteraction inventory;
    [Inject(Id = "PopUp")]
    GameObject popUp;

    private PopUp tableAskPopup;
    private TextMeshProUGUI tableAskText;
    
    // ���̺��� ���빰�� �����ϱ� ���� ������ Ŭ������ �ʿ���

    void Start()
    {
        tableItem = transform.GetChild(0).GetComponent<SpriteRenderer>();
        isHave = item == null ? false : true;
    }

    public override void Interaction(ItemData item)
    {  
        if (isHave)
        { 
            GiveItem();
        }

        else
        { 
            TakeItem(item);
        }
    }

    public override void UiOnOffMethod(Collision2D collision)
    {
        if (tableAskPopup == null)
        {
            //TODO: POPUP TEXT 구조 수정되면 GetComponent로 받아오는건 안해도 될듯
            tableAskPopup = popUp.GetComponent<PopUp>();
            tableAskText = popUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            tableAskPopup.objName = "가판대";
        }

        //TODO: 안내 멘트는 수정해야함. 
        if (item != null)
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
    /// �÷��̾ ���̺� ������ ������� ��
    /// </summary>
    /// <param name="item"></param>
    private void TakeItem(ItemData item)
    {
        this.item = item;
        tableItem.sprite = item.ItemImage;
        isHave = true;

    }

    /// <summary>
    /// �÷��̾ ���̺��� ������ ���� ��
    /// </summary>
    private void GiveItem()
    {
        ItemData item = this.item;
        this.item = null; 
        tableItem.sprite = null;

        inventory.AddItemToInventory(item);
        isHave = false;
    } 
}