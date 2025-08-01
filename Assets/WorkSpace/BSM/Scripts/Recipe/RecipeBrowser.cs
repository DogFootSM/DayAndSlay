using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBrowser : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown mainCategoryDropdown;
    [SerializeField] private TMP_Dropdown subCategoryDropdown;
    [SerializeField] private GameObject recipeListParent;
    [SerializeField] private GameObject recipeListElementPrefab;
    [SerializeField] private RecipeObserver recipeObserver;
    [SerializeField] private TMP_InputField recipeSearchInputField;
    [SerializeField] private Button searchButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Animator recipeToastAnimator;
    [SerializeField] private Button refreshButton;
    
    private static GameObject mismatchText;
    
    private ItemDatabaseManager itemDatabase => ItemDatabaseManager.instance;
    private List<ItemData> recipeList => itemDatabase.ItemDatabase.items;
    
    private List<string> mainWeaponSubCategoryOptions = new List<string>()
    {
        "전체", "활", "검", "창", "완드"
    };

    private List<string> subWeaponSubCategoryOptions = new List<string>()
    {
        "전체", "화살통", "미정이", "엠블렘", "마도서"
    };

    private List<string> allSubCategoryOptions = new List<string>()
    {
        "전체"
    };
    
    private List<string> armorSubCategoryOptions = new List<string>()
    {
        "전체", "모자", "상의", "하의", "장갑", "신발"
    };

    private List<string> accessorySubCategoryOptions = new List<string>()
    {
        "전체", "망토", "팔찌", "미정이2", "미정이3"
    };
    
    private int mainCategoryValue;
    private int subCategoryValue;
    private int recipeInputToastHash;

    private static int RecipeCount;
    private static int MisMatchCount;
    
    private void Awake()
    {
        mainCategoryDropdown.onValueChanged.AddListener(x => ChangedMainCategoryOption(x));
        subCategoryDropdown.onValueChanged.AddListener(x => ChangedSubCategoryRecipeList(x));
        deleteButton.onClick.AddListener(() => recipeSearchInputField.text = string.Empty);
        searchButton.onClick.AddListener(RecipeSearch);
        refreshButton.onClick.AddListener(RefreshRecipeList);
        recipeInputToastHash = Animator.StringToHash("ShowToast");
        mismatchText = transform.GetChild(2).GetChild(5).GetChild(0).GetChild(2).gameObject;
        
        InitRecipeList();
    }
 
    private void OnDisable()
    {   
        mainCategoryDropdown.value = 0;
        recipeSearchInputField.text = string.Empty;
    }

    private void ChangedSubCategoryRecipeList(int value)
    {
        subCategoryValue = value;
        MisMatchCount = 0;
        recipeObserver.ChangeSubCategory(subCategoryValue);
    }
    
    /// <summary>
    /// 레시피 리스트 초기화
    /// </summary>
    private void InitRecipeList()
    {
        for (int i = 0; i < recipeList.Count; i++)
        {
            if(!recipeList[i].IsEquipment) continue;
            CreateRecipes(recipeList[i]);
        }
        
        RecipeCount = recipeListParent.transform.childCount;
    }

    /// <summary>
    /// 아이템 이름 검색
    /// </summary>
    private void RecipeSearch()
    {
        if (recipeSearchInputField.text == string.Empty)
        {
            //검색어 입력 안내 토스트 메시지
            recipeToastAnimator.SetTrigger(recipeInputToastHash);
            return;
        }

        MisMatchCount = 0;
        recipeObserver.SearchItemName(recipeSearchInputField.text);
        recipeSearchInputField.text = string.Empty;
    }
    
    /// <summary>
    /// 대분류 변경에 따른 중분류 카테고리 변경
    /// </summary>
    /// <param name="value"></param>
    private void ChangedMainCategoryOption(int value)
    {
        subCategoryDropdown.ClearOptions();
        switch (value)
        {
            case 0:
                subCategoryDropdown.AddOptions(allSubCategoryOptions);
                break;
            case 1:
                subCategoryDropdown.AddOptions(mainWeaponSubCategoryOptions);
                break;
            case 2:
                subCategoryDropdown.AddOptions(subWeaponSubCategoryOptions);
                break;
            case 3:
                subCategoryDropdown.AddOptions(armorSubCategoryOptions);
                break;
            case 4:
                subCategoryDropdown.AddOptions(accessorySubCategoryOptions);
                break;
        }

        mainCategoryValue = value;
        MisMatchCount = 0;
        recipeObserver.ChangeMainCategory(mainCategoryValue);
        subCategoryDropdown.RefreshShownValue();
    }

    /// <summary>
    /// 레시피 리스트 새로고침
    /// </summary>
    private void RefreshRecipeList()
    {
        //현재 메인 카테고리가 '전체' 인 상태에서 새로고침
        if (mainCategoryValue == 0)
        {
            recipeObserver.ChangeMainCategory(mainCategoryValue);    
        }
        
        //메인 카테고리가 그 외인 상태일 경우
        mainCategoryDropdown.value = 0;
        mismatchText.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 초기 전체 아이템 레시피 리스트 생성
    /// </summary>
    /// <param name="recipeData"></param>
    private void CreateRecipes(ItemData recipeData)
    {
        GameObject recipeListElement = Instantiate(recipeListElementPrefab, transform.position, Quaternion.identity, recipeListParent.transform);
        RecipeElement recipeElement = recipeListElement.GetComponent<RecipeElement>();
        recipeElement.SetElement(recipeData);
    }

    /// <summary>
    /// 검색된 리스트 요소 개수 확인
    /// </summary>
    /// <param name="mismatch"></param>
    public static void MisMatchCountCheck(int mismatch)
    {
        MisMatchCount += mismatch; 
        
        if (RecipeCount - MisMatchCount <= 0)
        {
            mismatchText.gameObject.SetActive(true);
        }
        else
        {
            mismatchText.gameObject.SetActive(false);
        }
    }
    
}
