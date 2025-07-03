using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemData itemData;

    public int ItemId;
 
    private void Start()
    {
        itemData = BSM_ItemManager.ItemManager.instance.GetItemData(ItemId);
    }

}
