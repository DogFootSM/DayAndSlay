using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SkillSlotInvoker : MonoBehaviour
{
    private SkillFactory slotSkill;
    private SkillNode skillNode;
    private PlayerController playerController;
    
    private Vector2 curDirection = Vector2.down;
    public UnityAction<Vector2> OnDirectionChanged;

    private Coroutine markDashWaitCo;
    private KeyCode inputQuickSlotKey;
    private SkillCoolDown beforeChangedSkillCooldown;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

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
        skillNode = QuickSlotData.WeaponQuickSlotDict[weaponType][quickSlotType];
        
        //쿨다운이 리셋된 상태이며, 캐스팅이 진행중이지 않는 상태일 경우 스킬 사용
        if (skillNode.IsCoolDownReset && !skillNode.PlayerModel.IsCasting)
        {
            slotSkill = SkillFactoryManager.GetSkillFactory(skillNode);
 
            //특정 스킬을 사용했는지?
            if (slotSkill is SPAS008 spas008)
            {
                beforeChangedSkillCooldown = CoolDownUIHub.CoolDownImageMap[quickSlotType];
                
                //해당 스킬을 이미 사용해서 표식을 남겨놓은 상태인지 확인
                if (!skillNode.IsMarkOnTarget)
                {
                    //표식을 남겨놓은 상태가 아닐 경우 표식 설정
                    skillNode.SetMarkOnTarget(curDirection, transform.position);
                    
                    //스킬 사용 방향에 몬스터가 없을 경우 리턴
                    if (skillNode.GetMarkOnTarget() == null) return 0;
                    
                    if (markDashWaitCo != null)
                    {
                        StopCoroutine(markDashWaitCo);
                        markDashWaitCo = null;
                    }
                    
                    //스킬 사용 대기 효과 재생
                    QuickSlotWaitUseUI.QuickSlotWaitUses[quickSlotType].PlayAnimation();
                    markDashWaitCo = StartCoroutine(WaitForMarkDashInput(skillNode, quickSlotType));
                }
                else
                {
                    if (markDashWaitCo != null)
                    {
                        StopCoroutine(markDashWaitCo);
                        markDashWaitCo = null;
                    }

                    if (skillNode.GetMarkOnTarget() == null)
                    {
                        skillNode.IsCoolDownReset = false;
                        skillNode.IsMarkOnTarget = false;
                        
                        //퀵슬롯이 변경되었을 경우를 대비해 퀵슬롯 타입을 찾음
                        QuickSlotType type = CoolDownUIHub.SearchSkillCoolDown(beforeChangedSkillCooldown);
                    
                        CoolDownUIHub.CoolDownImageMap[type].SkillSlotUpdateCoolDown(skillNode);
                        QuickSlotWaitUseUI.QuickSlotWaitUses[type].StopAnimation();
                        return 0;
                    }
                    
                    //현재 캐릭터 위치와 타겟 몬스터 위치 비교하여 방향 보정
                    if (Mathf.Abs(curDirection.x) > Mathf.Abs(curDirection.y))
                    {
                        if (skillNode.GetMarkOnTarget().transform.position.x > transform.position.x)
                        {
                            curDirection = Vector2.right;
                        }
                        else
                        {
                            curDirection = Vector2.left;
                        } 
                    }
                    else
                    {
                        if (skillNode.GetMarkOnTarget().transform.position.y > transform.position.y)
                        {
                            curDirection = Vector2.up;
                        }
                        else
                        {
                            curDirection = Vector2.down;
                        } 
                    }
                    
                    //스킬 애니메이션 재생
                    playerController.BodyAnimator.Play(slotSkill.SendSkillAnimationHash(curDirection));
                    playerController.WeaponAnimator.Play(slotSkill.SendSkillAnimationHash(curDirection));
                    
                    slotSkill.UseSkill(curDirection, transform.position);
                    skillNode.IsCoolDownReset = false;
                    
                    //퀵슬롯이 변경되었을 경우를 대비해 퀵슬롯 타입을 찾음
                    QuickSlotType searchType = CoolDownUIHub.SearchSkillCoolDown(beforeChangedSkillCooldown);
                    
                    CoolDownUIHub.CoolDownImageMap[searchType].SkillSlotUpdateCoolDown(skillNode);
                    QuickSlotWaitUseUI.QuickSlotWaitUses[searchType].StopAnimation();
                    return skillNode.skillData.UseSkillDelay;
                }
            }
            //창 8번 표식 스킬 외 스킬 사용
            else
            {
                //스킬 애니메이션 재생
                playerController.BodyAnimator.Play(slotSkill.SendSkillAnimationHash(curDirection));
                playerController.WeaponAnimator.Play(slotSkill.SendSkillAnimationHash(curDirection));
                 
                slotSkill.UseSkill(curDirection, transform.position);
                skillNode.IsCoolDownReset = false;
                CoolDownUIHub.CoolDownImageMap[quickSlotType].SkillSlotUpdateCoolDown(skillNode);
                return skillNode.skillData.UseSkillDelay;
            }
        }
        else
        {
            //경고음 재생
            SoundManager.Instance.PlaySfx(SFXSound.COOLDOWN);
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
            
        //표식이 남은 몬스터가 있을 경우 해당 몬스터에 노출된 표식 이펙트 제거
        if (skillNode.GetMarkOnTarget() != null)
        {
            IEffectReceiver receiver = skillNode.GetMarkOnTarget().GetComponent<IEffectReceiver>();
            receiver.ReceiveMarkOnTarget();
        }

        QuickSlotType searchType = CoolDownUIHub.SearchSkillCoolDown(beforeChangedSkillCooldown);
        QuickSlotWaitUseUI.QuickSlotWaitUses[searchType].StopAnimation();
        skillNode.IsCoolDownReset = false;
        CoolDownUIHub.CoolDownImageMap[searchType].SkillSlotUpdateCoolDown(skillNode);
    }

    private void OnDrawGizmos()
    {
        if (slotSkill == null) return;
        
        slotSkill.Gizmos();
    }
}