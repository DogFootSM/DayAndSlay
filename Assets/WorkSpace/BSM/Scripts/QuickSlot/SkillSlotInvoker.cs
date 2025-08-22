using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SkillSlotInvoker : MonoBehaviour
{
    private SkillFactory slotSkill;

    private Vector2 curDirection = Vector2.down;
    public UnityAction<Vector2> OnDirectionChanged;

    private Coroutine markDashWaitCo;
    private KeyCode inputQuickSlotKey;

    private void OnEnable()
    {
        OnDirectionChanged += ChangedDirection;
    }

    private void OnDisable()
    {
        OnDirectionChanged -= ChangedDirection;
    }

    /// <summary>
    /// 스킬을 사용 시 타격될 방향 변경
    /// </summary>
    /// <param name="direction">스킬 사용 시 데미지를 줄 몬스터를 감지할 방향</param>
    private void ChangedDirection(Vector2 direction)
    {
        curDirection = direction;
    }

    /// <summary>
    /// 스킬 팩토리에 스킬 노드 전달
    /// </summary>
    /// <param name="quickSlotType">사용할 스킬이 등록된 퀵 슬롯 타입</param>
    /// <param name="weaponType">현재 캐릭터가 장착하고 있는 무기의 타입을 스킬의 키값으로 사용</param>
    /// <returns>해당 스킬의 후딜레이 시간</returns>
    public float InvokeSkillFromSlot(QuickSlotType quickSlotType, CharacterWeaponType weaponType)
    {
        SkillNode skillNode = QuickSlotData.WeaponQuickSlotDict[weaponType][quickSlotType];

        //TODO: 캐스팅 완료되기 전이면 스킬 사용x?
        if (skillNode.IsCoolDownReset)
        {
            slotSkill = SkillFactoryManager.GetSkillFactory(skillNode);

            if (slotSkill is SPAS008 spas008)
            {
                if (!skillNode.IsMarkOnTarget)
                {
                    skillNode.SetMarkOnTarget(curDirection, transform.position);

                    if (markDashWaitCo != null)
                    {
                        StopCoroutine(markDashWaitCo);
                        markDashWaitCo = null;
                    }
                    
                    markDashWaitCo = StartCoroutine(WaitForMarkDashInput(skillNode, quickSlotType));
                }
                else
                {
                    if (markDashWaitCo != null)
                    {
                        StopCoroutine(markDashWaitCo);
                        markDashWaitCo = null;
                    }

                    slotSkill.UseSkill(curDirection, transform.position);
                    skillNode.IsCoolDownReset = false;
                    CoolDownUIHub.CoolDownImageMap[quickSlotType].UpdateCoolDown(skillNode);

                    return skillNode.skillData.UseSkillDelay;
                }
            }
            else
            {
                slotSkill.UseSkill(curDirection, transform.position);
                skillNode.IsCoolDownReset = false;
                CoolDownUIHub.CoolDownImageMap[quickSlotType].UpdateCoolDown(skillNode);
                return skillNode.skillData.UseSkillDelay;
            }
        }
        else
        {
            //TODO: 스킬 쿨타임 사용 불가 UI 라던지 사용 불가 사운드 
            Debug.Log($"{skillNode.skillData.SkillName} 쿨타임 초기화 x");
        }

        return 0;
    }

    private IEnumerator WaitForMarkDashInput(SkillNode skillNode, QuickSlotType quickSlotType)
    {
        float elapsedTime = 5f;

        while (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            yield return null;
        }

        skillNode.IsMarkOnTarget = false;
        IEffectReceiver receiver = skillNode.GetMarkOnTarget().GetComponent<IEffectReceiver>();
        receiver.ReceiveMarkOnTarget();
        skillNode.IsCoolDownReset = false;
        CoolDownUIHub.CoolDownImageMap[quickSlotType].UpdateCoolDown(skillNode);
    }

    private void OnDrawGizmos()
    {
        if (slotSkill == null) return;

        //TODO: 스킬 기즈모 테스트용
        slotSkill.Gizmos();
    }
}