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

    private void Start()
    {
        Init();
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
    }
}
