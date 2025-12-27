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
        SoundManager.Instance.PlaySfx(SFXSound.NORMAL_ATTACK);
        attackHandler.NormalAttack(playerObject.transform.position, curEquippedItem, playerModel);
    }

    /// <summary>
    /// 백대쉬 공격
    /// </summary>
    /// <param name="direction"></param>
    public void BackDashAttack(Vector2 direction)
    {
        SoundManager.Instance.PlaySfx(SFXSound.NORMAL_ATTACK);
        attackHandler.BackDashAttack(playerObject.transform.position, direction, curEquippedItem, playerModel);
    }
    
    /// <summary>
    /// 공격할 방향 설정 애니메이션 이벤트
    /// </summary>
    public void SetDirectionEvent()
    {
        attackHandler.SetDirection(curDirection);
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        attackHandler.DrawGizmos();
    }
#endif
}