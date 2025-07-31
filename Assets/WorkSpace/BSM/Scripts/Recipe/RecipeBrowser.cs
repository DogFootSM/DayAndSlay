using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeBrowser : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown mainCategoryDropdown;
    [SerializeField] private TMP_Dropdown subCategoryDropdown;
    [SerializeField] private GameObject recipeListParent;
    [SerializeField] private GameObject recipeListElementPrefab;
    [SerializeField] private RecipeObserver recipeObserver;
    
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
    
    private Dictionary<Parts, List<ItemData>> searchRecipeMap = new Dictionary<Parts, List<ItemData>>();
    private List<RecipeElement> recipeElements = new List<RecipeElement>();

    private int mainCategoryValue;
    private int subCategoryValue;
    
    
    private void Awake()
    {
        mainCategoryDropdown.onValueChanged.AddListener(x => ChangedSubCategoryOption(x));
        subCategoryDropdown.onValueChanged.AddListener(x => ChangedSubCategoryRecipeList(x));
        
        for (int i = 0; i < recipeList.Count; i++)
        {
            if (!searchRecipeMap.ContainsKey(recipeList[i].Parts))
            {
                searchRecipeMap.Add(recipeList[i].Parts, new List<ItemData>());
            }
            
            if(!recipeList[i].IsEquipment) continue;
            
            searchRecipeMap[recipeList[i].Parts].Add(recipeList[i]);
            CreateRecipes(recipeList[i]);
        }
    }
 
    private void ChangedSubCategoryRecipeList(int value)
    {
        subCategoryValue = value;
        recipeObserver.ChangeSubCategory(subCategoryValue);
    }
    
    /// <summary>
    /// 대분류 변경에 따른 중분류 카테고리 변경
    /// </summary>
    /// <param name="value"></param>
    private void ChangedSubCategoryOption(int value)
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
        recipeObserver.ChangeMainCategory(mainCategoryValue);
        subCategoryDropdown.RefreshShownValue();
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
        recipeElements.Add(recipeElement);
    }
    
}
