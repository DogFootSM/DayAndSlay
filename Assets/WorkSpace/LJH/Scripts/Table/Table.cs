using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Table : MonoBehaviour, IInteractionStore
{
    //아이템 보유 여부
    private bool isHave;

    [SerializeField] Item item;
    Sprite itemImage;

    SpriteRenderer tableItem;


    void Start()
    {
        tableItem = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void Interaction()
    {
        Debug.Log("테이블 사용");
        //Todo: 테이블 사용
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Todo : E키 누르기 팝업창 노출
        //Todo : E키 누르면 테이블 아이템x && 플레이어 아이템 o 아이템 넣기
        //Todo : E키 누르면 테이블 아이템o && 플레이어 아이템 x 아이템 빼기
        //Todo : E키 누르면 테이블 아이템o && 플레이어 아이템 o 아무일 없음
        //Todo : E키 누르면 테이블 아이템x && 플레이어 아이템 x 아무일 없음

        
    }

    /// <summary>
    /// 플레이어가 아이템 집어넣을 때
    /// </summary>
    /// <param name="item"></param>
    public void TakeItem(Item item)
    {
        this.item = item;
        itemImage = item.GetComponent<Item>().itemData.ItemImage;
        tableItem.sprite = itemImage;

    }

    /// <summary>
    /// 플레이어가 아이템 꺼낼 때
    /// </summary>
    public void GiveItem()
    {
        item = null;
        itemImage = null;
        tableItem.sprite = null;
    }


}
