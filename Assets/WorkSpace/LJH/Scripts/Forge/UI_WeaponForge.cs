using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UI_WeaponForge : BaseUI
{
    //타입 버튼을 누르면 해당 타입에 존재하는 아이템 갯수만큼 버튼이 생기고 버튼의 이름이 그것들로 채워져야함
    //책 우측 탭을 선택하여 메인무기, 서브무기 변경 가능
    // 메인무기를 선택하면 타입에 메인무기 목록이 떠야함
    // 서브무기를 선택하면 타입에 서브무기 목록이 떠야함

    [SerializeField][SerializedDictionary]
    private SerializedDictionary<string, ItemData> weaponStorage;
    [SerializeField][SerializedDictionary]
    private SerializedDictionary<string, ItemData> subWeaponStorage;

    Dictionary<WeaponType, List<ItemData>> weaponDict = new Dictionary<WeaponType, List<ItemData>>();
    Dictionary<WeaponType, List<ItemData>> subWeaponDict = new Dictionary<WeaponType, List<ItemData>>();


    /// <summary>
    /// 무기, 보조 무기의 딕셔너리가 하나로 통합되어 있어 DictSize 3으로 설정, 추후 딕셔너리 분리 고려중
    /// </summary>
    private const int DICTSIZE = 3;


    private DictList<Button> tabButtonDictList = new DictList<Button>();
    private DictList<Button> typeButtonDictList = new DictList<Button>();
    private DictList<Button> itemButtonDictList = new DictList<Button>();
    private DictList<Button> setButtonDictList = new DictList<Button>();

    string[] weaponTypeArray = { "검", "창", "활", "지팡이" };
    string[] subWeaponTypeArray = { "방패", "엠블렘", "화살통", "마도서" };


    private void Start()
    {
        Init();
        ButtonInit();
    }


    /// <summary>
    /// 버튼 활성화 제어 (Controls activation buttons)
    /// </summary>
    /// <param name="buttonCounts"></param>
    private void ButtonActivate(int buttonCounts)
    {
        for (int i = 0; i < itemButtonDictList.Count; i++)
        {
            itemButtonDictList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < buttonCounts; i++)
        {
            itemButtonDictList[i].gameObject.SetActive(true);
        }
    }

    private void SetTypeButton(string[] typeArray)
    {
        //선택한 탭에 따라 타입 버튼명 변경 (무기 <=> 보조무기)
        for (int i = 0; i < weaponTypeArray.Length; i++)
        {
            typeButtonDictList[i].GetComponentInChildren<TextMeshProUGUI>().text = typeArray[i];
        }
    }

    private void SetItemButton(WeaponType weaponType)
    {
        //선택한 타입버튼에 따라 아이템 버튼 설정
        for (int i = 0; i < DICTSIZE; i++)
        {
            itemButtonDictList[i].GetComponent<ItemButton>().itemData = weaponDict[weaponType][i];
            itemButtonDictList[i].GetComponentInChildren<TextMeshProUGUI>().text = weaponDict[weaponType][i].name;
        }
    }

    /// <summary>
    /// 버튼 초기값 설정 (Sets initial button)
    /// </summary>
    private void ButtonInit()
    {
        SetTypeButton(weaponTypeArray);
        SetItemButton(WeaponType.SHORT_SWORD);

        ButtonActivate(DICTSIZE);
    }

    /// <summary>
    /// 리스트 설정 초기화 ( initial Button List)
    /// </summary>
    private void Init()
    {
        tabButtonDictList.Add("WeaponTab", GetUI<Button>("WeaponTab"));
        tabButtonDictList.Add("SubWeaponTab", GetUI<Button>("SubWeaponTab"));

        typeButtonDictList.Add("Type1", GetUI<Button>("Type1"));
        typeButtonDictList.Add("Type2", GetUI<Button>("Type2"));
        typeButtonDictList.Add("Type3", GetUI<Button>("Type3"));
        typeButtonDictList.Add("Type4", GetUI<Button>("Type4"));

        itemButtonDictList.Add("Item1", GetUI<Button>("Item1"));
        itemButtonDictList.Add("Item2", GetUI<Button>("Item2"));
        itemButtonDictList.Add("Item3", GetUI<Button>("Item3"));
        itemButtonDictList.Add("Item4", GetUI<Button>("Item4"));
        itemButtonDictList.Add("Item5", GetUI<Button>("Item5"));

        setButtonDictList.Add("OkayButton", GetUI<Button>("OkayButton"));
        setButtonDictList.Add("CancelButton", GetUI<Button>("CancelButton"));

        //WeaponType에 사용되지 않는 값이 있어 하드코딩으로 처리해둠
        DictMake(WeaponType.SHORT_SWORD);
        DictMake(WeaponType.SPEAR);
        DictMake(WeaponType.BOW);
        DictMake(WeaponType.WAND);
    }

    private void DictMake(WeaponType weaponType)
    {
        weaponDict[weaponType] = ListMake(weaponType);
    }

    private List<ItemData> ListMake(WeaponType weaponType)
    {
        List<ItemData> weaponList = new List<ItemData>();

        for (int i = 1; i <= DICTSIZE; i++)
        {
            weaponList.Add(weaponStorage[$"{weaponType}{i}"]);
        }
        return weaponList;
    }
}

