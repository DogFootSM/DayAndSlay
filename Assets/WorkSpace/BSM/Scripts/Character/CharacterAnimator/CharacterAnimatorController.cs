using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

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
  
    /// <summary>
    /// 무기 별 애니메이터 교체
    /// </summary>
    public void WeaponAnimatorChange(int curWeaponIndex)
    {
        WeaponAnimator.runtimeAnimatorController = WeaponAnimators[curWeaponIndex].runtimeAnimatorController;
        WeaponLibrary.spriteLibraryAsset = EquipmentLibraryAsset[curWeaponIndex];
    }
     
}
