using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    private IAttackHandler attackHandler;
    private CharacterWeaponType curWeaponType;

    public UnityAction<CharacterWeaponType> OnWeaponTypeChanged;
 
    private void OnEnable()
    {
        OnWeaponTypeChanged += GetCurrentWeaponType;
    }

    private void OnDisable()
    {
        OnWeaponTypeChanged -= GetCurrentWeaponType;
    }

    /// <summary>
    /// 무기 타입 변경에 따른 무기 핸들러 변경
    /// </summary>
    /// <param name="weaponType"></param>
    private void GetCurrentWeaponType(CharacterWeaponType weaponType)
    {
        attackHandler = AttackHandlerFactory.ChangeAttackType(weaponType); 
    }
    
    /// <summary>
    /// 무기 핸들러의 공격 호출
    /// </summary>
    public void Attack()
    {
        attackHandler.Attack();
    }
    
}
