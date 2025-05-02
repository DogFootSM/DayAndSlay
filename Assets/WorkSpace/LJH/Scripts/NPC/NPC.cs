using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public bool IsBuyer;


    //Todo : 상점에서 뚫린 아이템 목록을 가져와서 넣어줘야함
    [SerializeField]
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

        PickItem();
        GoStore();
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
