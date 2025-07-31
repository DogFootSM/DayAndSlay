using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class RecipeObserver : MonoBehaviour
{
    public static event Action<int> OnChangedMainCategory;
    public static event Action<int> OnChangedSubCategory;
    public static int mainCategory;
    
    public void ChangeMainCategory(int mainCategory)
    {
        RecipeObserver.mainCategory = mainCategory;
        OnChangedMainCategory?.Invoke(mainCategory);
    }

    public void ChangeSubCategory(int subCategory)
    {
        OnChangedSubCategory?.Invoke(subCategory);    
    }
}
