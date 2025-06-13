using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ArmorForge : BaseUI
{
    //추후 리팩토링 필요
    //현재 잘 돌아가긴 하나 손대야 할때 참담한 심정 느낄듯
    Animator animator;

    [SerializeField]
    [SerializedDictionary]
    private SerializedDictionary<string, ItemData> plateStorage;
    [SerializeField]
    [SerializedDictionary]
    private SerializedDictionary<string, ItemData> leatherStorage;
    [SerializeField]
    [SerializedDictionary]
    private SerializedDictionary<string, ItemData> clothStorage;

    Dictionary<ArmorType, List<ItemData>> plateDict = new Dictionary<ArmorType, List<ItemData>>();
    Dictionary<ArmorType, List<ItemData>> leatherDict = new Dictionary<ArmorType, List<ItemData>>();
    Dictionary<ArmorType, List<ItemData>> clothDict = new Dictionary<ArmorType, List<ItemData>>();


    /// <summary>
    /// 무기, 보조 무기의 딕셔너리가 하나로 통합되어 있어 DictSize 3으로 설정, 추후 딕셔너리 분리 고려중
    /// </summary>
    private const int DICTSIZE = 3;


    private DictList<Button> tabButtonDictList = new DictList<Button>();
    private DictList<Button> typeButtonDictList = new DictList<Button>();
    private DictList<Button> itemButtonDictList = new DictList<Button>();
    private DictList<Button> setButtonDictList = new DictList<Button>();

    string[] armorTypeArray = { "투구", "갑옷", "바지", "장갑", "신발" };


    List<ButtonWrapper> buttonWrappers = new List<ButtonWrapper>();

    List<ButtonWrapper> plateButtonWrappers = new List<ButtonWrapper>();
    List<ButtonWrapper> leatherButtonWrappers = new List<ButtonWrapper>();
    List<ButtonWrapper> clothButtonWrappers = new List<ButtonWrapper>();
    MaterialType materialType;

    List<ButtonWrapper> itemButtonWrappers = new List<ButtonWrapper>();

    private DictList<TextMeshProUGUI> prevTextDictList = new DictList<TextMeshProUGUI>();
    private Image prevItemImage;

    private void Start()
    {
        //초기 설정을 아예 제대로 잡아주는게 좋아보임
        Init();
        ButtonInit();
        TypeWrapperInit();
        DefaultSetting();
    }

    public void DefaultSetting()
    {
        TabButton(tabButtonDictList[0]);
        TypeButton(typeButtonDictList[0]);
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

        //선택한 탭에 따라 타입 버튼명 변경 (무기 <=> 보조무기)
        for (int i = 0; i < armorTypeArray.Length; i++)
        {
            int index = i;
            typeButtonDictList[i].onClick.RemoveAllListeners();
            typeButtonDictList[i].GetComponentInChildren<TextMeshProUGUI>().text = typeArray[i];
            if (materialType == MaterialType.PLATE)
            {
                typeButtonDictList[i].onClick.AddListener(() => TypeButton(plateButtonWrappers[index].button));
            }
            else if (materialType == MaterialType.LEATHER)
            {
                typeButtonDictList[i].onClick.AddListener(() => TypeButton(leatherButtonWrappers[index].button));
            }
            else if (materialType == MaterialType.CLOTH)
            {
                typeButtonDictList[i].onClick.AddListener(() => TypeButton(clothButtonWrappers[index].button));
            }
        }
    }
    /// <summary>
    /// 선택한 타입버튼에 따라 아이템 버튼 설정
    /// </summary>
    /// <param name="armorType"></param>
    private void SetItemButton(ArmorType armorType)
    {
        if (materialType == MaterialType.PLATE)
        {
            for (int i = 0; i < DICTSIZE; i++)
            {
                ItemData itemData = plateDict[armorType][i];

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
        }

        else if (materialType == MaterialType.LEATHER)
        {
            for (int i = 0; i < DICTSIZE; i++)
            {
                ItemData itemData = leatherDict[armorType][i];

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
        }
        else if (materialType == MaterialType.CLOTH)
        {
            for (int i = 0; i < DICTSIZE; i++)
            {
                ItemData itemData = clothDict[armorType][i];

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
        if (clickedButton == tabButtonDictList["PlateTab"])
        {
            animator.Play("BookNextPage");
            materialType = MaterialType.PLATE;
            
        }
        else if (clickedButton == tabButtonDictList["LeatherTab"])
        {
            animator.Play("BookNextPage");
            materialType = MaterialType.LEATHER;
        }
        else if (clickedButton == tabButtonDictList["ClothTab"])
        {
            animator.Play("BookNextPage");
            materialType = MaterialType.CLOTH;
        }
        SetTypeButton(armorTypeArray);
        TypeButton(typeButtonDictList[0]);
    }

    /// <summary>
    /// 타입 버튼 선택
    /// </summary>
    /// <param name="clickedButton"></param>
    private void TypeButton(Button clickedButton)
    {
        if (materialType == MaterialType.PLATE)
        {
            foreach (ButtonWrapper typeButtonWrapper in plateButtonWrappers)
            {
                if (clickedButton == typeButtonWrapper.button)
                {
                    SetItemButton(typeButtonWrapper.armorType);
                }
            }
        }

        else if (materialType == MaterialType.LEATHER)
        {
            foreach (ButtonWrapper typeButtonWrapper in leatherButtonWrappers)
            {
                if (clickedButton == typeButtonWrapper.button)
                {
                    SetItemButton(typeButtonWrapper.armorType);
                }
            }
        }

        else if (materialType == MaterialType.CLOTH)
        {
            foreach (ButtonWrapper typeButtonWrapper in clothButtonWrappers)
            {
                if (clickedButton == typeButtonWrapper.button)
                {
                    SetItemButton(typeButtonWrapper.armorType);
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
    private void ButtonInit()
    {
        SetItemButton(ArmorType.HELMET);
        ButtonActivate(DICTSIZE);
    }

    /// <summary>
    /// 래퍼 클래스 초기화 (Wrapper Class Initialize)
    /// </summary>
    private void TypeWrapperInit()
    {
        plateButtonWrappers = CreateNewWrapper();
        leatherButtonWrappers = CreateNewWrapper();
        clothButtonWrappers = CreateNewWrapper();
    }

    List<ButtonWrapper> CreateNewWrapper()
    {
        return new List<ButtonWrapper>()
            {
                new ButtonWrapper("투구", typeButtonDictList[0], ArmorType.HELMET),
                new ButtonWrapper("갑옷", typeButtonDictList[1], ArmorType.ARMOR),
                new ButtonWrapper("바지", typeButtonDictList[2], ArmorType.PANTS),
                new ButtonWrapper("장갑", typeButtonDictList[3], ArmorType.ARM),
                new ButtonWrapper("신발", typeButtonDictList[4], ArmorType.SHOES),
            };
    }

    /// <summary>
    /// 초기화 ( initial)
    /// </summary>
    private void Init()
    {
        tabButtonDictList.Add("PlateTab", GetUI<Button>("PlateTab"));
        tabButtonDictList.Add("LeatherTab", GetUI<Button>("LeatherTab"));
        tabButtonDictList.Add("ClothTab", GetUI<Button>("ClothTab"));

        typeButtonDictList.Add("Type1", GetUI<Button>("Type1"));
        typeButtonDictList.Add("Type2", GetUI<Button>("Type2"));
        typeButtonDictList.Add("Type3", GetUI<Button>("Type3"));
        typeButtonDictList.Add("Type4", GetUI<Button>("Type4"));
        typeButtonDictList.Add("Type5", GetUI<Button>("Type5"));

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

        WrapperMake(DICTSIZE, buttonWrappers, tabButtonDictList, TabButton);

        DictMake();
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

    private void DictMake()
    {
        Debug.Log("딕트메이크실행됨");
        for(int i = 0; i < (int)ArmorType.Size; i++)
        {
            plateDict[(ArmorType)i] = PlateListMake((ArmorType)i);
            leatherDict[(ArmorType)i] = LeatherListMake((ArmorType)i);
            clothDict[(ArmorType)i] = ClothListMake((ArmorType)i);
            Debug.Log(plateDict[(ArmorType)i]);
        }

    }


    private List<ItemData> PlateListMake(ArmorType armorType)
    {
        List<ItemData> plateArmorList = new List<ItemData>();

        for (int i = 1; i <= DICTSIZE; i++)
        {
            plateArmorList.Add(plateStorage[$"{armorType}{i}"]);
        }
        return plateArmorList;
    }

    private List<ItemData> LeatherListMake(ArmorType armorType)
    {
        List<ItemData> leatherArmorList = new List<ItemData>();

        for (int i = 1; i <= DICTSIZE; i++)
        {
            leatherArmorList.Add(leatherStorage[$"{armorType}{i}"]);
        }
        return leatherArmorList;
    }

    private List<ItemData> ClothListMake(ArmorType armorType)
    {
        List<ItemData> clothArmorList = new List<ItemData>();

        for (int i = 1; i <= DICTSIZE; i++)
        {
            clothArmorList.Add(clothStorage[$"{armorType}{i}"]);
        }
        return clothArmorList;
    }
}

