using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_haveItem : MonoBehaviour
{
    [SerializeField] temp_forgeUI forge;

    [Header("테스트용 아이템 숫자")]
    [SerializeField] int itemsCount;

    List<Item> items = new List<Item>();



    public void itemCountButton()
    {
        items.Clear();

        for (int i = 0; i < itemsCount; i++)
        {
            items.Add(new Item());
        }
        forge.CreateItemButtons(items.Count);
        Debug.Log($"아이템의 갯수 {items.Count}개");
    }
}
