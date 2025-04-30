using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    //아이템 보유 여부
    private bool isHave;

    GameObject item;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Todo : E키 누르기 팝업창 노출
        //Todo : E키 누르면 테이블 아이템x && 플레이어 아이템 o 아이템 넣기
        //Todo : E키 누르면 테이블 아이템o && 플레이어 아이템 x 아이템 빼기
        //Todo : E키 누르면 테이블 아이템o && 플레이어 아이템 o 아무일 없음
        //Todo : E키 누르면 테이블 아이템x && 플레이어 아이템 x 아무일 없음

        
    }

    public void TakeItem(GameObject item)
    {
        this.item = item;
    }

    public void GiveItem()
    {
        item = null;
    }

    public void showItem()
    {

    }
}
