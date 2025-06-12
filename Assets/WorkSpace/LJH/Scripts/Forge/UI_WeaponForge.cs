using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponForge : BaseUI
{
    //추후 리팩토링 필요
    //현재 잘 돌아가긴 하나 손대야 할때 참담한 심정 느낄듯
    Animator animator;

    [SerializeField]
    [SerializedDictionary]
    private SerializedDictionary<string, ItemData> weaponStorage;
    [SerializeField]
    [SerializedDictionary]
    private SerializedDictionary<string, ItemData> subWeaponStorage;

    //칼을 키로 쓰고 아이템을 밸류로 저장함
    //헬멧을 키로 쓰고 아이템을 밸류로 저장함
    Dictionary<WeaponType, List<ItemData>> weaponDict = new Dictionary<WeaponType, List<ItemData>>();
    Dictionary<SubWeaponType, List<ItemData>> subWeaponDict = new Dictionary<SubWeaponType, List<ItemData>>();


    /// <summary>
    /// 무기, 보조 무기의 딕셔너리가 하나로 통합되어 있어 DictSize 3으로 설정, 추후 딕셔너리 분리 고려중
    /// </summary>
    private const int DICTSIZE = 3;


    private DictList<Button> tabButtonDictList = new DictList<Button>();
    private DictList<Button> typeButtonDictList = new DictList<Button>();
    private DictList<Button> itemButtonDictList = new DictList<Button>();
    private DictList<Button> setButtonDictList = new DictList<Button>();

    string[] weaponTypeArray = { "검", "창", "활", "완드" };
    string[] subWeaponTypeArray = { "방패", "엠블렘", "화살통", "마도서" };


    List<ButtonWrapper> buttonWrappers = new List<ButtonWrapper>();

    List<ButtonWrapper> weaponButtonWrappers = new List<ButtonWrapper>();
    List<ButtonWrapper> subWeaponButtonWrappers = new List<ButtonWrapper>();
    EquipType equipType;

    List<ButtonWrapper> itemButtonWrappers = new List<ButtonWrapper>();

    private DictList<TextMeshProUGUI> prevTextDictList = new DictList<TextMeshProUGUI>();
    private Image prevItemImage;

    private void Start()
    {
        //초기 설정을 아예 제대로 잡아주는게 좋아보임
        Init();
        ButtonInit(weaponTypeArray);
        TypeWrapperInit();
        SetTypeButton(weaponTypeArray);
        ItemButton(itemButtonDictList[0]);
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

    /// <summary>
    /// 탭 버튼에서 실행, 타입 버튼의 이름과 가지고있는 버튼을 세팅해줌
    /// </summary>
    /// <param name="typeArray"></param>
    private void SetTypeButton(string[] typeArray)
    {
        ButtonInit(typeArray);

        //선택한 탭에 따라 타입 버튼명 변경 (무기 <=> 보조무기)
        for (int i = 0; i < weaponTypeArray.Length; i++)
        {
            int index = i;
            typeButtonDictList[i].onClick.RemoveAllListeners();
            typeButtonDictList[i].GetComponentInChildren<TextMeshProUGUI>().text = typeArray[i];
            if (equipType == EquipType.WEAPON)
            {
                typeButtonDictList[i].onClick.AddListener(() => TypeButton(weaponButtonWrappers[index].button));
            }
            else if(equipType == EquipType.SUBWEAPON)
            {
                typeButtonDictList[i].onClick.AddListener(() => TypeButton(subWeaponButtonWrappers[index].button));
            }
        }
    }
    /// <summary>
    /// 선택한 타입버튼에 따라 아이템 버튼 설정
    /// </summary>
    /// <param name="weaponType"></param>
    private void SetItemButton(WeaponType weaponType)
    {
        for (int i = 0; i < DICTSIZE; i++)
        {
            ItemData itemData = weaponDict[weaponType][i];

            Button button = itemButtonDictList[i];
            button.GetComponent<ItemButton>().itemData = itemData;
            button.GetComponentInChildren<TextMeshProUGUI>().text = itemData.name;

            if (i < itemButtonWrappers.Count)
            {
                itemButtonWrappers[i].itemData = itemData;
            }
            else
            {
                itemButtonWrappers.Add(new ButtonWrapper("아이템버튼", button, itemData));
            }

        }

        for (int i = 0; i < itemButtonDictList.Count; i++)
        {
            int index = i;
            itemButtonDictList[i].onClick.RemoveAllListeners();
            itemButtonDictList[i].onClick.AddListener(() => ItemButton(itemButtonWrappers[index].button));
        }
    }

    /// <summary>
    /// 선택한 타입버튼에 따라 아이템 버튼 설정
    /// </summary>
    /// <param name="subWeaponType"></param>
    private void SetItemButton(SubWeaponType subWeaponType)
    {
        for (int i = 0; i < DICTSIZE; i++)
        {
            ItemData itemData = subWeaponDict[subWeaponType][i];

            Button button = itemButtonDictList[i];
            button.GetComponent<ItemButton>().itemData = itemData;
            button.GetComponentInChildren<TextMeshProUGUI>().text = itemData.name;

            if (i < itemButtonWrappers.Count)
            {
                itemButtonWrappers[i].itemData = itemData;
            }
            else
            {
                itemButtonWrappers.Add(new ButtonWrapper("아이템버튼", button, itemData));
            }

        }

        for (int i = 0; i < itemButtonDictList.Count; i++)
        {
            int index = i;
            itemButtonDictList[i].onClick.RemoveAllListeners();
            itemButtonDictList[i].onClick.AddListener(() => ItemButton(itemButtonWrappers[index].button));
        }
    }

    /// <summary>
    /// 탭 버튼 선택
    /// </summary>
    /// <param name="clickedButton"></param>
    private void TabButton(Button clickedButton)
    {

        if (clickedButton == tabButtonDictList["WeaponTab"])
        {
            animator.Play("BookNextPage");
            equipType = EquipType.WEAPON;
            SetTypeButton(weaponTypeArray);
        }
        else if (clickedButton == tabButtonDictList["SubWeaponTab"])
        {
            animator.Play("BookNextPage");
            equipType = EquipType.SUBWEAPON;
            SetTypeButton(subWeaponTypeArray);
        }
    }

    /// <summary>
    /// 타입 버튼 선택
    /// </summary>
    /// <param name="clickedButton"></param>
    private void TypeButton(Button clickedButton)
    {
        if (equipType == EquipType.WEAPON)
        {
            foreach (ButtonWrapper typeButtonWrapper in weaponButtonWrappers)
            {
                if (clickedButton == typeButtonWrapper.button)
                {
                    SetItemButton(typeButtonWrapper.weaponType);
                }
            }
        }

        else if(equipType == EquipType.SUBWEAPON)
        {
            foreach (ButtonWrapper typeButtonWrapper in subWeaponButtonWrappers)
            {
                if (clickedButton == typeButtonWrapper.button)
                {
                    SetItemButton(typeButtonWrapper.subWeaponType);
                }
            }
        }
    }

    /// <summary>
    /// 아이템 버튼 선택
    /// </summary>
    /// <param name="clickedButton"></param>
    private void ItemButton(Button clickedButton)
    {
        foreach (ButtonWrapper itemButtonWrapper in itemButtonWrappers)
        {
            if (clickedButton == itemButtonWrapper.button)
            {
                prevItemImage.sprite = itemButtonWrappers[0].itemData.ItemImage;

                prevTextDictList["PrevName"].text = itemButtonWrapper.itemData.Name;
                prevTextDictList["ATK"].text = itemButtonWrapper.itemData.Attack.ToString();
                prevTextDictList["DEF"].text = itemButtonWrapper.itemData.Defence.ToString();
                prevTextDictList["HP"].text = itemButtonWrapper.itemData.Hp.ToString();
                prevTextDictList["Ingrediant1"].text = itemButtonWrapper.itemData.Name;
                prevTextDictList["Ingrediant2"].text = itemButtonWrapper.itemData.Name.ToString();
                prevTextDictList["Ingrediant3"].text = itemButtonWrapper.itemData.Name.ToString();
                prevTextDictList["Ingrediant4"].text = itemButtonWrapper.itemData.Name.ToString();
            }
        }
    }

    /// <summary>
    /// 버튼 초기값 설정 (Sets initial button)
    /// </summary>
    private void ButtonInit(string[] typeArray)
    {

        if (typeArray == weaponTypeArray)
        {
            SetItemButton(WeaponType.SHORT_SWORD);
        }
        else if (typeArray == subWeaponTypeArray)
        {
            SetItemButton(SubWeaponType.SHIELD);
        }

        ButtonActivate(DICTSIZE);
    }

    /// <summary>
    /// 래퍼 클래스 초기화 (Wrapper Class Initialize)
    /// </summary>
    private void TypeWrapperInit()
    {
            weaponButtonWrappers = new List<ButtonWrapper>()
            {
                new ButtonWrapper("검", typeButtonDictList[0], WeaponType.SHORT_SWORD),
                new ButtonWrapper("창", typeButtonDictList[1], WeaponType.SPEAR),
                new ButtonWrapper("활", typeButtonDictList[2], WeaponType.BOW),
                new ButtonWrapper("완드", typeButtonDictList[3], WeaponType.WAND)
            };


            subWeaponButtonWrappers = new List<ButtonWrapper>()
            {
                new ButtonWrapper("방패", typeButtonDictList[0], SubWeaponType.SHIELD),
                new ButtonWrapper("엠블렘", typeButtonDictList[1], SubWeaponType.EMBLEM),
                new ButtonWrapper("화살통", typeButtonDictList[2], SubWeaponType.ARROW),
                new ButtonWrapper("마도서", typeButtonDictList[3], SubWeaponType.BOOK)
            };
    }

    /// <summary>
    /// 초기화 ( initial)
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

        prevTextDictList.Add("PrevName", GetUI<TextMeshProUGUI>("ItemName"));
        prevTextDictList.Add("ATK", GetUI<TextMeshProUGUI>("ATK"));
        prevTextDictList.Add("DEF", GetUI<TextMeshProUGUI>("DEF"));
        prevTextDictList.Add("HP", GetUI<TextMeshProUGUI>("HP"));
        prevTextDictList.Add("Ingrediant1", GetUI<TextMeshProUGUI>("Ingrediant1"));
        prevTextDictList.Add("Ingrediant2", GetUI<TextMeshProUGUI>("Ingrediant2"));
        prevTextDictList.Add("Ingrediant3", GetUI<TextMeshProUGUI>("Ingrediant3"));
        prevTextDictList.Add("Ingrediant4", GetUI<TextMeshProUGUI>("Ingrediant4"));

        prevItemImage = GetUI<Image>("ItemImage");

        WrapperMake(2, buttonWrappers, tabButtonDictList, TabButton);



        //WeaponType에 사용되지 않는 값이 있어 하드코딩으로 처리해둠
        DictMake(WeaponType.SHORT_SWORD);
        DictMake(WeaponType.SPEAR);
        DictMake(WeaponType.BOW);
        DictMake(WeaponType.WAND);

        DictMake(SubWeaponType.SHIELD); 
        DictMake(SubWeaponType.EMBLEM);
        DictMake(SubWeaponType.ARROW);
        DictMake(SubWeaponType.BOOK);

        animator = GetComponent<Animator>();

    }

    void WrapperMake(int size, List<ButtonWrapper> buttonWrappers, DictList<Button> dictList, Action<Button> action)
    {
        for (int i = 0; i < size; i++)
        {
            buttonWrappers.Add(new ButtonWrapper("temp_name", dictList[i]));
            Button Bbutton = buttonWrappers[i].button;
            Bbutton.onClick.AddListener(() => action(Bbutton));
        }
    }

    private void DictMake(WeaponType weaponType)
    {
        weaponDict[weaponType] = ListMake(weaponType);
    }

    private void DictMake(SubWeaponType subWeaponType)
    {
        subWeaponDict[subWeaponType] = ListMake(subWeaponType);
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

    private List<ItemData> ListMake(SubWeaponType subWeaponType)
    {
        List<ItemData> subWeaponList = new List<ItemData>();

        for (int i = 1; i <= DICTSIZE; i++)
        {
            subWeaponList.Add(subWeaponStorage[$"{subWeaponType}{i}"]);
        }
        return subWeaponList;
    }
}

