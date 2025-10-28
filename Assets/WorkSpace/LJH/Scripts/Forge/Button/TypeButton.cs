using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeButton : MonoBehaviour
{
    [SerializeField] private List<TypeButton> typeButtons;
    public string typeName;

    private TextMeshProUGUI buttonName;
    private List<ItemData> itemDataList =  new List<ItemData>();
    [SerializeField] private List<ItemButton> itemButtonList;

    private void Start()
    {
        buttonName = GetComponentInParent<TextMeshProUGUI>();
        SetItemButtons(typeButtons.IndexOf(this));

        GetComponent<Button>().onClick.AddListener(() => SetItemButtons(typeButtons.IndexOf(this)));
    }
    
    /// <summary>
    /// 버튼 설정(다른 클래스에서 사용)
    /// </summary>
    public void SetThisButton(string _typeName)
    {
        buttonName.text = typeName;
    }

    public void SetItemButtons(int _typeButton)
    {
        List<ItemData> items = new List<ItemData>();

        items = ItemDatabaseManager.instance.GetAllEquipItem();

        foreach (ItemData item in items)
        {
            if (item.WeaponType == (WeaponType)_typeButton && item.ItemId % 2 == 0)
            {
                itemDataList.Add(item);
            }
        }

        for (int i = 0; i < itemButtonList.Count; i++)
        {
            itemButtonList[i].SetButtonItem(itemDataList[i]);
        }
    }
}
