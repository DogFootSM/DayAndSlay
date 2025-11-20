using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    
    public UnityAction<CharacterWeaponType, ItemData, PlayerModel> OnWeaponTypeChanged;
    public UnityAction<Vector2> OnDirectionChanged; 
    
    private IAttackHandler attackHandler;
    private CharacterWeaponType curWeaponType;
    private PlayerModel playerModel;
    private Vector2 curDirection;
    private ItemData curEquippedItem;
    
    private void Awake()
    {
        curDirection = Vector2.down;
    }

    private void OnEnable()
    {
        OnWeaponTypeChanged += GetCurrentWeaponType;
        OnDirectionChanged += ChangedMoveDirection;
    }

    private void OnDisable()
    {
        OnWeaponTypeChanged -= GetCurrentWeaponType;
        OnDirectionChanged -= ChangedMoveDirection;
    }

    /// <summary>
    /// 무기 타입 변경에 따른 무기 핸들러 변경
    /// </summary>
    /// <param name="weaponType"></param>
    private void GetCurrentWeaponType(CharacterWeaponType weaponType, ItemData itemData, PlayerModel playerModel)
    {
        attackHandler = AttackHandlerFactory.ChangeAttackType(weaponType);
        curEquippedItem = itemData;
        this.playerModel = playerModel;
    }

    /// <summary>
    /// 스킬을 사용 시 타격될 방향 변경
    /// </summary>
    /// <param name="direction">스킬 사용 시 데미지를 줄 몬스터를 감지할 방향</param>
    private void ChangedMoveDirection(Vector2 direction)
    {
        curDirection = direction; 
    }
    
    /// <summary>
    /// 무기 핸들러의 기본 공격 호출
    /// </summary>
    public void NormalAttack()
    {
        Debug.Log($"현재 장착 무기 :{curEquippedItem.Name}");
        attackHandler.NormalAttack(curDirection, playerObject.transform.position, curEquippedItem, playerModel);
    }

    private void OnDrawGizmos()
    {  
        //attackHandler.DrawGizmos(); 
    }
}
