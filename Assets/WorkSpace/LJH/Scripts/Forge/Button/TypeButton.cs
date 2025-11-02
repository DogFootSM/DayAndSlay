using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeButton : MonoBehaviour
{
    //추후 인젝트로 변경
    [SerializeField] private ForgeCanvas forge;
    [SerializeField] private List<TypeButton> typeButtons;
    
    private TextMeshProUGUI buttonName;
    private List<ItemData> itemDataList =  new List<ItemData>();
    [SerializeField] private List<ItemButton> itemButtonList;
    
    [Header("ArmorForge")]
    [SerializeField] private MaterialType_kr material;

    private void Awake()
    {
        buttonName = GetComponentInChildren<TextMeshProUGUI>();

        GetComponent<Button>().onClick.AddListener(() => SetItemButtons(typeButtons.IndexOf(this)));
    }

    private void Start()
    {
        SetItemButtons(typeButtons.IndexOf(this));
    }
    
    /// <summary>
    /// 버튼 설정(다른 클래스에서 사용)
    /// </summary>
    public void SetThisButton(string typeName)
    {
        buttonName.text = typeName;
        SetItemButtons(typeButtons.IndexOf(this));
    }

    public void SetItemButtons(int _typeButton)
    {
        
        itemDataList.Clear(); 
    
        List<ItemData> items = ItemDatabaseManager.instance.GetAllEquipItem();

        foreach (ItemData item in items)
        {
            if (IsItemMatch(item, _typeButton, forge.GetCurParts()))
            {
                itemDataList.Add(item);
            }
        }

        // 아이템 버튼을 설정
        for (int i = 0; i < itemButtonList.Count; i++)
        {
            if (i < itemDataList.Count)
            {
                itemButtonList[i].SetButtonItem(itemDataList[i]);
                itemButtonList[i].gameObject.SetActive(true);
            }
            else
            {
                itemButtonList[i].gameObject.SetActive(false);
            }
        }
    }
    
    private bool IsItemMatch(ItemData item, int typeButtonIndex, Parts_kr currentParts)
    {
        bool isMaterialMatch = MaterialCheck(item.ItemId, typeButtonIndex);
    
        switch (currentParts)
        {
            case Parts_kr.무기:
                return item.WeaponType == (WeaponType)typeButtonIndex && item.ItemId % 2 == 0;
    
            case Parts_kr.보조무기:
                return item.SubWeaponType == (SubWeaponType)typeButtonIndex && item.ItemId % 2 == 0;

        
            case Parts_kr.투구:
                return item.Parts == Parts.HELMET && isMaterialMatch;
            case Parts_kr.갑옷:
                return item.Parts == Parts.ARMOR && isMaterialMatch;
            case Parts_kr.바지:
                return item.Parts == Parts.PANTS && isMaterialMatch;
            case Parts_kr.장갑:
                return item.Parts == Parts.ARM && isMaterialMatch;
            case Parts_kr.신발:
                return item.Parts == Parts.SHOES && isMaterialMatch;
        
            default:
                return false;
        }
    }

    private bool MaterialCheck(int itemId, int typeIndex)
    {
        int matType = GetMaterialType(itemId);
    
        switch (typeIndex)
        {
            case (int)MaterialType_kr.중갑: 
                return matType == 13;
            case (int)MaterialType_kr.가죽: 
                return matType == 12;
            case (int)MaterialType_kr.천:   
                return matType == 11;
            default:
                return false;
        }
    }

    public int GetMaterialType(int itemID) => itemID / 10000;
}
