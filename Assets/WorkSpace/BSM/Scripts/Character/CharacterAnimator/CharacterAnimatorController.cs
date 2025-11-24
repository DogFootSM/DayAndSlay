using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D.Animation;
using Zenject;

public class CharacterAnimatorController : MonoBehaviour
{
    [Header("캐릭터 애니메이션 라이브러리")] [Tooltip("1. HAIR, 2. BODY, 3. SHIRT")]
    public SpriteLibraryAsset[] BodyLibraryAsset;

    [Header("캐릭터 무기 라이브러리")] 
    public SpriteLibraryAsset EquipmentLibraryAsset;

    [SerializeField] private SpriteLibrary weaponLibrary;
    [SerializeField] private SpriteLibrary bodyLibrary;
    [SerializeField] private SpriteLibrary hairLibrary;
    [SerializeField] private SpriteLibrary shirtsLibrary;
    
    [Header("무기 애니메이션 컨트롤러 컴포넌트")] [SerializeField]
    private Animator WeaponAnimator;

    [Header("무기 애니메이터 리스트")] [SerializeField]
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
    public void AnimatorChange(int curWeaponIndex, int weaponTier, bool inventoryChange = false)
    {
        if (!WeaponAnimator.gameObject.activeSelf)
        {
            WeaponAnimator.gameObject.SetActive(true);
        }
        //TODO: 모자, 망토 등 추후 업데이트 시 추가

        //무기 착용 상태
        if ((CharacterWeaponType)curWeaponIndex != CharacterWeaponType.EMPTY)
        {
            //인벤토리에서 무기 변경 상태
            if (inventoryChange)
            {
                //Body, Shirt, Hair 공격 스프라이트 라이브러리 변경
                dataManager.ChangeAttackSpriteLibraryAsset(BodyLibraryAsset, curWeaponIndex);
                dataManager.ChangeWeaponSpriteLibraryAsset(EquipmentLibraryAsset, curWeaponIndex, weaponTier);
                
                weaponLibrary.RefreshSpriteResolvers();
                bodyLibrary.RefreshSpriteResolvers();
                hairLibrary.RefreshSpriteResolvers();
                shirtsLibrary.RefreshSpriteResolvers();
            } 
        } 
        
        //캐릭터 Body, Hair, Shirt 애니메이션 컨트롤러 변경
        characterAnimator.runtimeAnimatorController = CharacterAnimators[curWeaponIndex].runtimeAnimatorController;
        WeaponAnimator.runtimeAnimatorController = WeaponAnimators[curWeaponIndex].runtimeAnimatorController;
    }

    /// <summary>
    /// 현재 무기의 애니메이터 및 라이브러리 변경
    /// </summary>
    /// <param name="curWeaponIndex"></param>
    /// <param name="weaponTier"></param>
    public void ChangeWeaponAnimator(int curWeaponIndex, int weaponTier)
    {
        if ((CharacterWeaponType)curWeaponIndex != CharacterWeaponType.EMPTY)
        {
            dataManager.ChangeWeaponSpriteLibraryAsset(EquipmentLibraryAsset, curWeaponIndex, weaponTier);
            weaponLibrary.RefreshSpriteResolvers();
            bodyLibrary.RefreshSpriteResolvers();
            hairLibrary.RefreshSpriteResolvers();
            shirtsLibrary.RefreshSpriteResolvers();
        }
        else
        {
            characterAnimator.runtimeAnimatorController = CharacterAnimators[curWeaponIndex].runtimeAnimatorController;
            WeaponAnimator.runtimeAnimatorController = WeaponAnimators[curWeaponIndex].runtimeAnimatorController;
        }
    }
}