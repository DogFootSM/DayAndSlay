using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDatabaseManager : MonoBehaviour
{
    public static ItemDatabaseManager instance;

    public ItemDatabase ItemDatabase
    {
        get => itemDatabase;
    }

    [SerializeField] private ItemDatabase itemDatabase;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public ItemData GetItemByID(int ID) => itemDatabase.GetItemByID(ID);

    public ItemData GetItemByName(string name) => GetItemByID(itemDatabase.GetItemByName(name));

    /// <summary>
    /// 장비 아이템 한번에 가져오기
    /// </summary>
    /// <returns></returns>
    public List<ItemData> GetAllEquipItem()
    {
        List<ItemData> list = new List<ItemData>();

        foreach (ItemData item in itemDatabase.items)
        {
            if (item.IsEquipment)
            {
                list.Add(item);
            }
        }

        return list
            .Where(item => item.IsEquipment)
            .OrderBy(item => item.ItemId)
            .ToList();
    }

    /// <summary>
    /// Forge에서 사용하는 무기 목록 가져오기
    /// </summary>
    /// <returns></returns>
    public List<ItemData> GetNormalWeaponItem()
    {
        List<ItemData> list = new List<ItemData>();

        foreach (ItemData item in itemDatabase.items)
        {
            if (item.IsEquipment && item.Parts == Parts.WEAPON && item.ItemId % 10 == 2)
            {
                list.Add(item);
            }
        }

        return list.OrderBy(item => item.ItemId).ToList();
    }

    public List<ItemData> GetSubWeaponItem()
    {
        List<ItemData> list = new List<ItemData>();

        foreach (ItemData item in itemDatabase.items)
        {
            if (item.IsEquipment && item.Parts == Parts.SUBWEAPON)
            {
                list.Add(item);
            }
        }
        
        return list.OrderBy(item => item.ItemId).ToList();
    }

    /// <summary>
    /// 재료 아이템 한번에 가져오기
    /// </summary>
    /// <returns></returns>
    public List<ItemData> GetAllIngrediantItem()
    {
        List<ItemData> list = new List<ItemData>();

        foreach (ItemData item in itemDatabase.items)
        {
            if (!item.IsEquipment)
            {
                list.Add(item);
            }
        }

        return list;
    }


    /// <summary>
    /// 원하는 무기타입의 아이템을 가져오는 함수
    /// </summary>
    /// <param name="weapon">무기 타입 입력</param>
    /// <returns></returns>
    public List<ItemData> GetWantTypeItem(WeaponType weapon)
    {
        List<ItemData> list = new List<ItemData>();

        foreach (ItemData item in GetNormalWeaponItem())
        {
            if (item.WeaponType == weapon)
            {
                list.Add(item);
            }
        }
        
        return list.OrderBy(item => item.ItemId).ToList();
    }
    
    /// <summary>
    /// 원하는 보조무기타입의 아이템을 가져오는 함수
    /// </summary>
    /// <param name="subweapon">보조 무기 타입 입력</param>
    /// <returns></returns>
    public List<ItemData> GetWantTypeItem(SubWeaponType subweapon)
    {
        List<ItemData> list = new  List<ItemData>();

        foreach (ItemData item in GetSubWeaponItem())
        {
            if (item.SubWeaponType == subweapon)
            {
                list.Add(item);
            }
        }
        
        return list.OrderBy(item => item.ItemId).ToList();
    }
    
    /// <summary>
    /// 원하는 방어구 재질의 아이템을 가져오는 함수
    /// </summary>
    /// <param name="armorParts">방어구 재질 입력</param>
    /// <returns></returns>
    public List<ItemData> GetWantTypeItem(MaterialType armorMaterial)
    {
        List<ItemData> list = new List<ItemData>();

        
        foreach (ItemData item in GetAllEquipItem())
        {
            if (item.MaterialType == armorMaterial)
            {
                list.Add(item);
            }
        }
        
        return list.OrderBy(item => item.ItemId).ToList();
    }
    
}