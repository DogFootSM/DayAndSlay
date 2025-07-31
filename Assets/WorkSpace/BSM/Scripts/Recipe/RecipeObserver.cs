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
    
    public static List<Parts> MainCategoryType = new List<Parts>();
    public static List<Enum> SubCategoryType = new List<Enum>();
    
    /// <summary>
    /// 메인 카테고리 변경 시 각 요소들에 전달
    /// </summary>
    /// <param name="mainCategory"></param>
    public void ChangeMainCategory(int mainCategory)
    {
        MainCategoryType.Clear();
        MainCategory = mainCategory;
        
        if (mainCategory == 1)
        { 
            MainCategoryType.Add(Parts.WEAPON);
        }
        else if (mainCategory == 2)
        { 
            MainCategoryType.Add(Parts.SUBWEAPON);
        }
        else if (mainCategory == 3)
        { 
            MainCategoryType.Add(Parts.SUBWEAPON);
            MainCategoryType.Add(Parts.ARM);
            MainCategoryType.Add(Parts.ARMOR);
            MainCategoryType.Add(Parts.PANTS);
            MainCategoryType.Add(Parts.SHOES);
        }
        else if (mainCategory == 4)
        {
            //TODO: 악세
        } 
        
        OnChangedMainCategory?.Invoke(mainCategory);
    }

    /// <summary>
    /// 서브 카테고리 변경 시 각 요소들에 전달
    /// </summary>
    /// <param name="subCategory"></param>
    public void ChangeSubCategory(int subCategory)
    {
        SubCategoryType.Clear();
        
        if (MainCategory == 1)
        {
            SubCategoryType.Add((WeaponType)subCategory -1); 
        }
        else if (MainCategory == 2)
        {
            SubCategoryType.Add((SubWeaponType)subCategory - 1);
        }
        else if (MainCategory == 3)
        {
            SubCategoryType.Add(ArmorType.HELMET);
            SubCategoryType.Add(ArmorType.ARMOR);
            SubCategoryType.Add(ArmorType.ARM);
            SubCategoryType.Add(ArmorType.PANTS);
            SubCategoryType.Add(ArmorType.SHOES);
        }
        else if (MainCategory == 4)
        {
            //TODO: 악세
        }
        
        OnChangedSubCategory?.Invoke(subCategory);    
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
