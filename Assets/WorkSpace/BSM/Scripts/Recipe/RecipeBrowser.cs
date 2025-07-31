using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeBrowser : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown mainCategoryDropdown;
    [SerializeField] private TMP_Dropdown subCategoryDropdown;
    
    private ItemDatabaseManager itemDatabase => ItemDatabaseManager.instance;
    private List<ItemRecipe> recipeList => itemDatabase.ItemDatabase.recipes;
    
    private List<string> mainWeaponSubCategoryOptions = new List<string>()
    {
        "검", "창", "활", "완드"
    };

    private List<string> subWeaponSubCategoryOptions = new List<string>()
    {
        "미정이", "엠블렘", "화살통", "마도서" 
    };

    private List<string> allSubCategoryOptions = new List<string>()
    {
        "전체"
    };
    
    private List<string> armorSubCategoryOptions = new List<string>()
    {
        "모자", "상의", "하의", "장갑", "신발"
    };

    private List<string> accessorySubCategoryOptions = new List<string>()
    {
        "망토", "미정이1", "미정이2", "미정이3"
    };
    
    private Dictionary<string, int> searchRecipeMap = new Dictionary<string, int>();

    private void Awake()
    {
        mainCategoryDropdown.onValueChanged.AddListener(x => ChangedSubCategoryOption(x));
    }

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
        subCategoryDropdown.RefreshShownValue();
    }
    
}
