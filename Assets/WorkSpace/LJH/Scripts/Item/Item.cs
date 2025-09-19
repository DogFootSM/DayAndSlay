using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemData itemData;

    public int ItemId;

    public void SetItem(ItemData itemData)
    {
        gameObject.name = itemData.Name;
        this.itemData = itemData;
        ItemId = itemData.ItemId;
    }
    
    private void Start()
    {
        itemData = ItemDatabaseManager.instance.GetItemByID(ItemId);
    }

}
