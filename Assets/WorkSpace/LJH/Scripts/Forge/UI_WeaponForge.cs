using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponForge : BaseUI
{
    [SerializeField] private List<Button> typeButtons = new List<Button>();
    [SerializeField] private List<Button> itemButtons = new List<Button>();

    private Button okayButton;
    private Button cancelButton;

    private string[] swordName = new string[3] { "조악한 검", "예리한 검", "전설의 검"};
    private string[] spearName = new string[3] { "조악한 창", "뾰족한 창", "전설의 창" };
    private string[] bowName = new string[3] { "조악한 활", "매서운 활", "전설의 활" };
    private string[] wandName = new string[3] { "조악한 완드", "신비한 완드", "전설의 완드" };

    //무기 별 레시피 리스트
    [SerializeField] private List<ItemRecipe> swordRecipes = new List<ItemRecipe>();
    [SerializeField] private List<ItemRecipe> spearRecipes = new List<ItemRecipe>();
    [SerializeField] private List<ItemRecipe> bowRecipes = new List<ItemRecipe>();
    [SerializeField] private List<ItemRecipe> wandRecipes = new List<ItemRecipe>();

    [SerializeField] private List<Item> swordList;
    [SerializeField] private List<Item> spearList;
    [SerializeField] private List<Item> bowList;
    [SerializeField] private List<Item> wandList;

    [SerializeField] private List<Item> shieldList;
    [SerializeField] private List<Item> emblemList;
    [SerializeField] private List<Item> arrowList;
    [SerializeField] private List<Item> boolList;


    //타입 버튼을 누르면 해당 타입에 존재하는 아이템 갯수만큼 버튼이 생기고 버튼의 이름이 그것들로 채워져야함
    //책 우측 탭을 선택하여 메인무기, 서브무기 변경 가능
    // 메인무기를 선택하면 타입에 메인무기 목록이 떠야함
    // 서브무기를 선택하면 타입에 서브무기 목록이 떠야함





    private void Start()
    {
        Init();
        CreateItemButtons(swordList);
    }


    /// <summary>
    /// 버튼 초기화
    /// </summary>
    /// <param name="itemsCount"></param>
    public void CreateItemButtons(List<Item> itemList)
    {
        int itemsCount = itemList.Count;

        foreach (var item in itemButtons)
        {
            item.gameObject.SetActive(false);
        }
        for (int i = 0; i < itemsCount; i++)
        {
            itemButtons[i].gameObject.SetActive(true);
        }
    }



    /// <summary>
    /// 타입 버튼 클릭시 아이템 목록 이름 변경 함수
    /// </summary>
    /// <param name="itemname"></param>
    void ItemButtonClick(string[] itemname)
    {
        Debug.Log("실행됨 소드버튼");
        for (int i = 0; i < itemButtons.Count; i++)
        {
            itemButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = itemname[i];

        }
    }

    /// <summary>
    /// 타입 버튼 클릭시 아이템 목록 SO 변경 함수
    /// </summary>
    /// <param name="itemname"></param>
    void ItemButtonSOInsert(List<ItemRecipe> itemRecipes)
    {
        for (int i = 0; i < itemButtons.Count; i++)
        {
            itemButtons[i].GetComponent<ItemButton>().itemRecipe = itemRecipes[i];
        }
    }


    public void ItemCreate()
    {
        //Todo : 아이템 생성돼서 인벤토리 넣어짐
    }

    public void CloseForgeUI()
    {
        gameObject.SetActive(false);
    }


    void Init()
    {
        typeButtons.Add(GetUI<Button>("TypeButton1"));
        typeButtons.Add(GetUI<Button>("TypeButton2"));
        typeButtons.Add(GetUI<Button>("TypeButton3"));
        typeButtons.Add(GetUI<Button>("TypeButton4"));

        itemButtons.Add(GetUI<Button>("ItemButton1"));
        itemButtons.Add(GetUI<Button>("ItemButton2"));
        itemButtons.Add(GetUI<Button>("ItemButton3"));

        okayButton = GetUI<Button>("OkayButton");
        cancelButton = GetUI<Button>("CancelButton");

        typeButtons[0].onClick.AddListener(() => { ItemButtonClick(swordName); ItemButtonSOInsert(swordRecipes); });
        typeButtons[1].onClick.AddListener(() => { ItemButtonClick(spearName); ItemButtonSOInsert(spearRecipes); });
        typeButtons[2].onClick.AddListener(() => { ItemButtonClick(bowName); ItemButtonSOInsert(bowRecipes); });
        typeButtons[3].onClick.AddListener(() => { ItemButtonClick(wandName); ItemButtonSOInsert(wandRecipes); });

        //okayButton.onClick.AddListener() => ItemCreate();
        cancelButton.onClick.AddListener(CloseForgeUI);
    }
}
