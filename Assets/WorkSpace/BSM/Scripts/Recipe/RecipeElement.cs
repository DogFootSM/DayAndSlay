using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeElement : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

    private LayoutElement layoutElement;

    private List<Parts> armorTypeOptions = new List<Parts>()
    {
        Parts.ARM,
        Parts.ARMOR,
        Parts.PANTS,
        Parts.SHOES,
        Parts.HELMET,
    };

    private Parts curParts;
    private WeaponType weaponType;
    private SubWeaponType subWeaponType;

    private void Awake()
    {
        layoutElement = GetComponent<LayoutElement>();
    }

    private void OnEnable()
    {
        RecipeObserver.OnChangedMainCategory += UpdateVisibilityByMainCategory;
        RecipeObserver.OnChangedSubCategory += UpdateVisibilityBySubCategory;
    }

    private void OnDisable()
    {
        RecipeObserver.OnChangedMainCategory -= UpdateVisibilityByMainCategory;
        RecipeObserver.OnChangedSubCategory -= UpdateVisibilityBySubCategory;
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
    }

    /// <summary>
    /// Main Category 선택에 따른 리스트 업데이트
    /// </summary>
    /// <param name="selectedCategory"></param>
    private void UpdateVisibilityByMainCategory(int selectedCategory)
    {
        //메인 카테고리
        // 0: 전체, 1: 주무기 , 2: 보조무기, 3: 방어구, 4: 악세
        //서브 카테고리
        // 0: 전체, 1.
        // if (selectedCategory == 0)
        // {
        //     gameObject.SetActive(true);
        //     return;
        // } 
        switch (selectedCategory)
        {
            case 0:
                transform.localScale = Vector3.one;
                layoutElement.ignoreLayout = false;
                break;
            case 1:
                transform.localScale = curParts == Parts.WEAPON ? Vector3.one : Vector3.zero;
                layoutElement.ignoreLayout = curParts != Parts.WEAPON;
                break;
            case 2:
                transform.localScale = curParts == Parts.SUBWEAPON ? Vector3.one : Vector3.zero;
                layoutElement.ignoreLayout = curParts != Parts.SUBWEAPON;
                break;
            case 3:
                transform.localScale = armorTypeOptions.Contains(curParts) ? Vector3.one : Vector3.zero;
                layoutElement.ignoreLayout = !armorTypeOptions.Contains(curParts);
                break;
            case 4:
                //TODO: 악세서리 타입에 대해 정의 필요
                break;
        }
    }

    /// <summary>
    /// Sub Category 선택에 따른 리스트 업데이트
    /// </summary>
    /// <param name="value"></param>
    private void UpdateVisibilityBySubCategory(int value)
    {
        //대분류가 주무기 상태
        if (RecipeObserver.mainCategory == 1)
        {
            //현재 파츠가 웨폰
            if (curParts == Parts.WEAPON)
            {
                if (value == 0)
                {
                    transform.localScale = Vector3.one;
                    layoutElement.ignoreLayout = false;
                }
                else
                {
                    transform.localScale = weaponType == (WeaponType)value - 1 ? Vector3.one : Vector3.zero;
                    layoutElement.ignoreLayout = weaponType != (WeaponType)value - 1;
                }
            }
        }
        //대분류가 보조무기 상태
        else if (RecipeObserver.mainCategory == 2)
        {
            //파츠가 보조 무기
            if (curParts == Parts.SUBWEAPON)
            {
                if (value == 0)
                {
                    transform.localScale = Vector3.one;
                    layoutElement.ignoreLayout = false;
                }
                else
                {
                    transform.localScale = subWeaponType == (SubWeaponType)value - 1 ? Vector3.one : Vector3.zero;
                    layoutElement.ignoreLayout = subWeaponType != (SubWeaponType)value - 1;
                }
            }
        }
        //대분류가 방어구인 상태
        else if (RecipeObserver.mainCategory == 3)
        {
            //파츠가 방어 아이템 옵션 중 속함
            if (armorTypeOptions.Contains(curParts))
            {
                if (value == 0)
                {
                    transform.localScale = Vector3.one;
                    layoutElement.ignoreLayout = false;
                }
                else
                { 
                    //파츠 타입을 정수로 변환 후 Armor타입의 시작 값에 맞춰 -2를 뺀 값과 비교
                    transform.localScale = (int)(curParts - 2) == value - 1 ? Vector3.one : Vector3.zero;
                    layoutElement.ignoreLayout = (int)(curParts - 2) != value - 1;
                }
            } 
        }
        else if (RecipeObserver.mainCategory == 4)
        {
            //TODO:악세 타입
        }
        
    }
}