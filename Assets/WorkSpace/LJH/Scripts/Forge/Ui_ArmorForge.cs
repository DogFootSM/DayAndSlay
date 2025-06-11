using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ArmorForge : BaseUI
{
    /*
    //타입 버튼을 누르면 해당 타입에 존재하는 아이템 갯수만큼 버튼이 생기고 버튼의 이름이 그것들로 채워져야함
    //책 우측 탭을 선택하여 메인무기, 서브무기 변경 가능
    // 메인무기를 선택하면 타입에 메인무기 목록이 떠야함
    // 서브무기를 선택하면 타입에 서브무기 목록이 떠야함

    [SerializeField]
    [SerializedDictionary]
    private SerializedDictionary<string, ItemData> plateStorage;
    [SerializeField]
    [SerializedDictionary]
    private SerializedDictionary<string, ItemData> leatherStorage;
    [SerializeField]
    [SerializedDictionary]
    private SerializedDictionary<string, ItemData> clothStorage;

    Dictionary<Parts, List<ItemData>> plateDict = new Dictionary<Parts, List<ItemData>>();
    Dictionary<Parts, List<ItemData>> leatherDict = new Dictionary<Parts, List<ItemData>>();
    Dictionary<Parts, List<ItemData>> clothDict = new Dictionary<Parts, List<ItemData>>();




    /// <summary>
    /// 무기, 보조 무기의 딕셔너리가 하나로 통합되어 있어 DictSize 3으로 설정, 추후 딕셔너리 분리 고려중
    /// </summary>
    private const int DICTSIZE = 3;


    private DictList<Button> tabButtonDictList = new DictList<Button>();
    private DictList<Button> typeButtonDictList = new DictList<Button>();
    private DictList<Button> itemButtonDictList = new DictList<Button>();
    private DictList<Button> setButtonDictList = new DictList<Button>();

    //타입
    string[] armorTypeArray = { "투구", "갑옷", "바지", "장갑", "신발" };



    List<ButtonWrapper> partsButtonWrappers = new List<ButtonWrapper>();

    List<ButtonWrapper> itemButtonWrappers = new List<ButtonWrapper>();

    private DictList<TextMeshProUGUI> prevTextDictList = new DictList<TextMeshProUGUI>();
    private Image prevItemImage;

    private void Start()
    {
        Init();
        ButtonInit();
        WrapperInit();
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
        //선택한 탭에 따라 타입 버튼명 변경 (중갑 <=> 가죽 <=> 천)
        for (int i = 0; i < armorTypeArray.Length; i++)
        {
            typeButtonDictList[i].GetComponentInChildren<TextMeshProUGUI>().text = typeArray[i];
        }
    }

    private void SetItemButton(Parts parts)
    {
        //선택한 타입버튼에 따라 아이템 버튼 설정
        for (int i = 0; i < DICTSIZE; i++)
        {
            itemButtonDictList[i].GetComponent<ItemButton>().itemData = plateDict[parts][i];
            itemButtonDictList[i].GetComponentInChildren<TextMeshProUGUI>().text = plateDict[parts][i].name;
        }
    }


    private void SetPreview()
    {

    }

    private void TabButton(Button clickedButton)
    {
        //중갑, 가죽, 천 중에 고르는 기능
        if (clickedButton == tabButtonDictList["Plate"])
        {
            SetTypeButton(armorTypeArray);
        }
        else if (clickedButton == tabButtonDictList["Leather"])
        {
            SetTypeButton(armorTypeArray);
        }
        else if (clickedButton == tabButtonDictList["Cloth"])
        {
            SetTypeButton(armorTypeArray);
        }

        WrapperInit();
    }

    private void TypeButton(Button clickedButton)
    {
        //옷 부위 고르는 기능
        foreach (ButtonWrapper typeButtonWrapper in partsButtonWrappers)
        {
            if (clickedButton == typeButtonWrapper.button)
            {
                SetItemButton(typeButtonWrapper.parts);
            }
        }

    }

    private void ItemButton(Button clickedButton)
    {
        foreach (ButtonWrapper itemButtonWrapper in itemButtonWrappers)
        {
            if (clickedButton == itemButtonWrapper.button)
            {
                prevItemImage.sprite = itemButtonWrapper.itemData.ItemImage;

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
        SetTypeButton(armorTypeArray);
        SetItemButton(Parts.HELMET);

        ButtonActivate(DICTSIZE);
    }

    /// <summary>
    /// 래퍼 클래스 초기화 (Wrapper Class Initialize)
    /// </summary>
    private void WrapperInit()
    {
        partsButtonWrappers = new List<ButtonWrapper>()
            {
                new ButtonWrapper(typeButtonDictList[0], Parts.HELMET),
                new ButtonWrapper(typeButtonDictList[1], Parts.ARMOR),
                new ButtonWrapper(typeButtonDictList[2], Parts.PANTS),
                new ButtonWrapper(typeButtonDictList[3], Parts.ARM),
                new ButtonWrapper(typeButtonDictList[4], Parts.SHOES)
            };


        for (int i = 0; i < partsButtonWrappers.Count; i++)
        {
            Button _typeButtonHelmet = partsButtonWrappers[i].button;
            _typeButtonHelmet.onClick.AddListener(() => TypeButton(_typeButtonHelmet));
        }


        for (int i = 0; i < DICTSIZE; i++)
        {
            itemButtonWrappers.Add(new ButtonWrapper(itemButtonDictList[i]));
            Button _itemButton = itemButtonWrappers[i].button;
            _itemButton.onClick.AddListener(() => ItemButton(_itemButton));
        }
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


        //Parts에 사용되지 않는 값이 있어 하드코딩으로 처리해둠
        DictMake(plateDict, Parts.HELMET, plateStorage);
        DictMake(plateDict, Parts.ARMOR, plateStorage);
        DictMake(plateDict, Parts.PANTS, plateStorage);
        DictMake(plateDict, Parts.ARM, plateStorage);
        DictMake(plateDict, Parts.SHOES, plateStorage);

        DictMake(leatherDict, Parts.HELMET, leatherStorage);
        DictMake(leatherDict, Parts.ARMOR, leatherStorage);
        DictMake(leatherDict, Parts.PANTS, leatherStorage);
        DictMake(leatherDict, Parts.ARM, leatherStorage);
        DictMake(leatherDict, Parts.SHOES, leatherStorage);

        DictMake(clothDict, Parts.HELMET, clothStorage);
        DictMake(clothDict, Parts.ARMOR, clothStorage);
        DictMake(clothDict, Parts.PANTS, clothStorage);
        DictMake(clothDict, Parts.ARM, clothStorage);
        DictMake(clothDict, Parts.SHOES, clothStorage);

    }

    private void DictMake(Dictionary<Parts, List<ItemData>>dict, Parts parts, SerializedDictionary<string, ItemData>storage)
    {
        dict[parts] = ListMake(storage, parts);
    }

    private List<ItemData> ListMake(SerializedDictionary<string, ItemData> storage, Parts parts)
    {
        List<ItemData> armorList = new List<ItemData>();

        for (int i = 1; i <= DICTSIZE; i++)
        {
            armorList.Add(storage[$"{parts}{i}"]);
        }
        return armorList;
    }*/
}

