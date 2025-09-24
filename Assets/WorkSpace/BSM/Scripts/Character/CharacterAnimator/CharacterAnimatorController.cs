using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Zenject;

public class CharacterAnimatorController : MonoBehaviour
{
    [Header("캐릭터 애니메이션 라이브러리")] [Tooltip("1. HAIR, 2. BODY, 3. SHIRT")]
    public SpriteLibraryAsset[] BodyLibraryAsset;
 
    [Header("캐릭터 장비 라이브러리")] [Tooltip("1. BOW, 2. SHORT_SWORD, 3.SPEAR")]
    public SpriteLibraryAsset[] EquipmentLibraryAsset;
    
    [SerializeField] private SpriteLibrary WeaponLibrary;
    
    [Header("무기 애니메이션 컨트롤러 컴포넌트")]
    [SerializeField] private Animator WeaponAnimator;
    
    [Header("무기 애니메이터 리스트")]     //TODO: BOW, SPEAR 애니메이터 추가 필요
    [SerializeField] private List<Animator> WeaponAnimators;

    /*TODO: 화살통, 무기 등급에 따라 스프라이트 변경될 경우 무기별 라이브러리 에셋 추가해서 변경하도록 하면됨
            미리 라이브러리 에셋 만들어 두면 됨
    */
    
    /// <summary>
    /// 무기 별 애니메이터 교체
    /// </summary>
    public void AnimatorChange(int curWeaponIndex)
    {
        //TODO: 무기 장착해제 했을 때 처리
        if (curWeaponIndex == 4)
        {   
            WeaponAnimator.gameObject.SetActive(false);
            return;
        }

        if (!WeaponAnimator.gameObject.activeSelf)
        {
            WeaponAnimator.gameObject.SetActive(true);
        }
        //TODO: 무기 교체했을 때 몸, 모자, 옷 애니메이터 컨트롤러 및 스프라이트 라이브러리 에셋 교체 필요

        //화살통 라이브러리 추가 필요
        WeaponAnimator.runtimeAnimatorController = WeaponAnimators[curWeaponIndex].runtimeAnimatorController;
        WeaponLibrary.spriteLibraryAsset = EquipmentLibraryAsset[curWeaponIndex];
    }
}
