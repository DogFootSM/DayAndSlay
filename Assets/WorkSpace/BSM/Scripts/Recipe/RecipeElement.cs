using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class RecipeElement : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Animator recipeAnimator;
    
    private ItemData itemData;
    private LayoutElement layoutElement;
    private Parts curParts;
    private WeaponType weaponType;
    private SubWeaponType subWeaponType;
    private string curItemName;
    private bool isArmorCheck;
    private bool isWeaponCheck;
    private bool isSubWeaponCheck;
    private bool isMainCategoryCheck;
    private int expandAniHash;
    
    private void Awake()
    {
        layoutElement = GetComponent<LayoutElement>(); 
        expandAniHash = Animator.StringToHash("isExpand");
        recipeAnimator.keepAnimatorStateOnDisable = true;
    }

    private void OnEnable()
    {
        RecipeObserver.OnChangedMainCategory += UpdateVisibilityByMainCategory;
        RecipeObserver.OnChangedSubCategory += UpdateVisibilityBySubCategory;
        RecipeObserver.OnSearchItemName += UpdateVisibilityBySearch;
        recipeAnimator.Rebind();
    }

    private void OnDisable()
    {
        RecipeObserver.OnChangedMainCategory -= UpdateVisibilityByMainCategory;
        RecipeObserver.OnChangedSubCategory -= UpdateVisibilityBySubCategory;
        RecipeObserver.OnSearchItemName -= UpdateVisibilityBySearch;
        
        //레시피 창 종료 시 초기화 작업 진행
        transform.localScale = Vector3.one;
        layoutElement.ignoreLayout = false;
    }

    /// <summary>
    /// 레시피 요소 데이터 설정
    /// </summary>
    public void SetElement(ItemData itemData)
    {
        itemImage.sprite = itemData.ItemImage;
        itemName.text = itemData.Name;
        itemDescription.text = itemData.ItemDescA;
        curParts = itemData.Parts;
        weaponType = itemData.WeaponType;
        subWeaponType = itemData.SubWeaponType;
        curItemName = itemData.Name;
        
        this.itemData = itemData;
    }

    /// <summary>
    /// Main Category 선택에 따른 리스트 업데이트
    /// </summary>
    /// <param name="selectedCategory"></param>
    private void UpdateVisibilityByMainCategory(int selectedCategory)
    {
        if (selectedCategory == 0)
        {
            transform.localScale = Vector3.one;
            layoutElement.ignoreLayout = false;
        }
        else
        {
            isMainCategoryCheck = RecipeObserver.MainCategoryTypeSet.Contains(curParts);
            ApplyVisibilityResult(isMainCategoryCheck);
        } 
    }

    /// <summary>
    /// Sub Category 선택에 따른 리스트 업데이트
    /// </summary>
    /// <param name="value"></param>
    private void UpdateVisibilityBySubCategory(int value)
    { 
        if (value == 0)
        {
            bool isCheck = RecipeObserver.MainCategoryTypeSet.Contains(curParts);
            ApplyVisibilityResult(isCheck);
        }
        else
        {
            isArmorCheck = RecipeObserver.SubCategoryTypeSet.Contains((ArmorType)(curParts - 2));
            isWeaponCheck = RecipeObserver.SubCategoryTypeSet.Contains(weaponType);
            isSubWeaponCheck = RecipeObserver.SubCategoryTypeSet.Contains(subWeaponType);
            
            bool isCheck = isMainCategoryCheck && (isArmorCheck || isWeaponCheck || isSubWeaponCheck);
            ApplyVisibilityResult(isCheck);
        } 
    }
 
    /// <summary>
    /// 입력한 검색어에 대한 리스트 탐색 후 필터링
    /// </summary>
    /// <param name="itemName"></param>
    private void UpdateVisibilityBySearch(string itemName)
    {
        bool isCheck = isMainCategoryCheck && (isArmorCheck || isWeaponCheck || isSubWeaponCheck);
        bool searchNameCheck = curItemName.Contains(itemName);
        
        if (RecipeObserver.MainCategory == 0)
        {
            ApplyVisibilityResult(searchNameCheck);
        }
        else
        {
            if (RecipeObserver.SubCategoryTypeSet.Count > 0)
            {
                ApplyVisibilityResult(searchNameCheck && isCheck);
            }
            else
            {
                ApplyVisibilityResult(searchNameCheck && isMainCategoryCheck);
            } 
        }  
    }
    
    /// <summary>
    /// 결과에 따른 오브젝트 노출 여부 설정
    /// </summary>
    /// <param name="visibility"></param>
    private void ApplyVisibilityResult(bool visibility)
    {
        transform.localScale = visibility ? Vector3.one : Vector3.zero;
        layoutElement.ignoreLayout = !visibility; 
        RecipeBrowser.MisMatchCountCheck(!visibility ? 1 : -1);
    }
    
    /// <summary>
    /// 아이템 데이터를 반환
    /// </summary>
    /// <returns></returns>
    public ItemData GetItemData()
    {
        return this.itemData;
    }

    /// <summary>
    /// 레시피 선택 시 영역 확대
    /// </summary>
    public void PanelExpandHeight()
    {
        recipeAnimator.SetBool(expandAniHash, true);
    }
    
    /// <summary>
    /// 레시피 영역 축소
    /// </summary>
    public void PanelShrinkHeight()
    {
        recipeAnimator.SetBool(expandAniHash, false);
    }
    
}