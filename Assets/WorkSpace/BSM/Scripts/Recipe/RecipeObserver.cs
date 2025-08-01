using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class RecipeObserver : MonoBehaviour
{
    public static event Action<int> OnChangedMainCategory;
    public static event Action<int> OnChangedSubCategory;
    public static event Action<string> OnSearchItemName; 
    
    public static int MainCategory;

    public static HashSet<Parts> MainCategoryTypeSet = new HashSet<Parts>();
    public static HashSet<Enum> SubCategoryTypeSet = new HashSet<Enum>();
    
    /// <summary>
    /// 메인 카테고리 변경 시 각 요소들에 전달
    /// </summary>
    /// <param name="mainCategoryValue"></param>
    public void ChangeMainCategory(int mainCategoryValue)
    {
        MainCategory = mainCategoryValue;
        MainCategoryTypeSet.Clear();
        
        if (mainCategoryValue == 1)
        {
            MainCategoryTypeSet.Add(Parts.WEAPON);
        }
        else if (mainCategoryValue == 2)
        { 
            MainCategoryTypeSet.Add(Parts.SUBWEAPON);
        }
        else if (mainCategoryValue == 3)
        { 
            MainCategoryTypeSet.Add(Parts.HELMET);
            MainCategoryTypeSet.Add(Parts.ARM);
            MainCategoryTypeSet.Add(Parts.ARMOR);
            MainCategoryTypeSet.Add(Parts.PANTS);
            MainCategoryTypeSet.Add(Parts.SHOES);
        }
        else if (mainCategoryValue == 4)
        {
            //TODO: 악세
        } 
        
        OnChangedMainCategory?.Invoke(mainCategoryValue);
    }

    /// <summary>
    /// 서브 카테고리 변경 시 각 요소들에 전달
    /// </summary>
    /// <param name="subCategoryValue"></param>
    public void ChangeSubCategory(int subCategoryValue)
    {
        SubCategoryTypeSet.Clear();
        
        if (MainCategory == 1)
        {
            SubCategoryTypeSet.Add((WeaponType)subCategoryValue - 1);
        }
        else if (MainCategory == 2)
        {
            SubCategoryTypeSet.Add((SubWeaponType)subCategoryValue - 1);
        }
        else if (MainCategory == 3)
        {
            SubCategoryTypeSet.Add((ArmorType)subCategoryValue -1);
        }
        else if (MainCategory == 4)
        {
            //TODO: 악세
        }
        
        OnChangedSubCategory?.Invoke(subCategoryValue);    
    }
    
    /// <summary>
    /// 입력한 이름 각 레시피 요소들에 전달
    /// </summary>
    /// <param name="name"></param>
    public void SearchItemName(string name)
    {
        OnSearchItemName?.Invoke(name);
    }
}
