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

    [Header("무기 애니메이션 컨트롤러 컴포넌트")] [SerializeField]
    private Animator WeaponAnimator;

    [Header("무기 애니메이터 리스트")]
    [SerializeField]
    private List<Animator> WeaponAnimators;

    [Header("캐릭터 애니메이션 리스트")] [SerializeField]
    private List<Animator> CharacterAnimators;

    [Header("캐릭터 애니메이션 컨트롤러 컴포넌트")] [SerializeField]
    private Animator characterAnimator;

    [Inject] private DataManager dataManager;

    private void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
    }

    /// <summary>
    /// 무기 별 애니메이터 교체
    /// </summary>
    public void AnimatorChange(int curWeaponIndex, bool isChange = false)
    {
        if (!WeaponAnimator.gameObject.activeSelf)
        {
            WeaponAnimator.gameObject.SetActive(true);
        }
        //TODO: 무기 교체했을 때 몸, 모자, 옷 애니메이터 컨트롤러 및 스프라이트 라이브러리 에셋 교체 필요

        //무기 착용 상태
        if ((CharacterWeaponType)curWeaponIndex != CharacterWeaponType.EMPTY)
        {
            //인벤토리 내에서 무기 변경 시에 공격 애니메이션 라이브러리 변경
            if (isChange)
            {
                //Body, Shirt, Hair 공격 스프라이트 라이브러리 변경
                dataManager.ChangeAttackSpriteLibraryAsset(BodyLibraryAsset, curWeaponIndex);
            }
            
            //캐릭터 Body, Hair, Shirt 애니메이션 컨트롤러 변경
            characterAnimator.runtimeAnimatorController = CharacterAnimators[curWeaponIndex].runtimeAnimatorController;

            //TODO: 추후 다 완성되면 무기도 라이브러리 변경이 아닌 컨트롤러만 변경하도록 변경하고 무기 변경시마다 무기 스프라이트 라이브러리 바꿔주기, 화살통, 무기 등급도
            //화살통 라이브러리 추가 필요
            WeaponAnimator.runtimeAnimatorController = WeaponAnimators[curWeaponIndex].runtimeAnimatorController;
            WeaponLibrary.spriteLibraryAsset = EquipmentLibraryAsset[curWeaponIndex];
        }
        else
        {
            //무기 미착용 상태
            characterAnimator.runtimeAnimatorController = CharacterAnimators[curWeaponIndex].runtimeAnimatorController;
            WeaponAnimator.runtimeAnimatorController = WeaponAnimators[curWeaponIndex].runtimeAnimatorController;
            WeaponLibrary.spriteLibraryAsset = EquipmentLibraryAsset[0];
        }
    }

}