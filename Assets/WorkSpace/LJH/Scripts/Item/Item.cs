using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemData itemData;

    public int ItemId;


    private void Start()
    {
        //해당 코드는 이전에 왜 추가했었는지가 불분명하여 일단 주석화 처리함
        //itemData = BSM_ItemManager.ItemManager.instance.GetItemData(ItemId);
    }

}
