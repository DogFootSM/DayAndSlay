using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NPC : MonoBehaviour
{
    public bool IsBuyer;

    [Inject]
    ItemManager itemManager;

    private List<Item> wantItemList = new List<Item>();
    private Item wantItem;

    private void Start()
    {
        NpcBehaviour();
    }


    void NpcBehaviour()
    {
        if (!IsBuyer)
        {
            DontGoStore();
            return;
        }

        ItemListSetting(itemManager.ItemList);
        PickItem();
        GoStore();
    }

    /// <summary>
    /// 아이템매니저에서 현재 판매 가능한 아이템 리스트 넣어줌
    /// </summary>
    /// <param name="itemList"></param>
    private void ItemListSetting(List<Item> itemList)
    {
        wantItemList = new List<Item>(itemList);
    }


    /// <summary>
    /// 어떤 아이템을 원하는지 설정
    /// </summary>
    void PickItem()
    {
        int itemIndex = Random.Range(0, wantItemList.Count);
        wantItem = wantItemList[itemIndex];
    }


    void GoStore()
    {
        //Todo : 상점으로 이동하는 엔피씨
        Debug.Log("아이템 구매할래");
    }

    void DontGoStore()
    {
        //Todo : 상점으로 가지 않는 엔피씨
        Debug.Log("아이템 구매안할래");
    }
}
