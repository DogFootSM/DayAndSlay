using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class temp_forgeUI : MonoBehaviour
{
    [SerializeField] temp_haveItem haveItem;
    [SerializeField] List<Button> buttonList;

    public void CreateItemButtons(int itemsCount)
    {
        foreach (var item in buttonList)
        {
            item.gameObject.SetActive(false);
        }
        for (int i = 0; i < itemsCount; i++)
        {
            buttonList[i].gameObject.SetActive(true);
        }
    }
}
