using System;
using System.Collections;
using System.Collections.Generic;
using ModestTree.Util;
using UnityEngine;

public class WDAS004 : MeleeSkill
{
    private Action action;
    private Vector2 hitPos;
    private Coroutine castingCo;
    
    public WDAS004(SkillNode skillNode) : base(skillNode)
    {
        leftHash = Animator.StringToHash("SkillMotion_Left_2");
        rightHash = Animator.StringToHash("SkillMotion_Right_2");
        upHash = Animator.StringToHash("SkillMotion_Up_2");
        downHash = Animator.StringToHash("SkillMotion_Down_2");
    } 
    
    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        skillDamage = GetSkillDamage();
        ExecuteCasting(skillNode.skillData.SkillCastingTime);
        hitPos = playerPosition + (direction * (skillNode.skillData.SkillRadiusRange / 2));
        
        action = () => ExecutePostCastAction(playerPosition);
        castingCo = skillNode.PlayerSkillReceiver.StartCoroutine(WaitCastingRoutine(action)); 
    }

    /// <summary>
    /// 캐스팅 이후 수행할 동작
    /// </summary>
    /// <param name="direction">캐릭터가 스킬 사용시 바라본 방향</param>
    /// <param name="playerPosition">캐릭터가 스킬을 사용한 위치</param>
    private void ExecutePostCastAction(Vector2 playerPosition)
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(hitPos, overlapSize, 0, monsterLayer);
        Sort.SortMonstersByNearest(cols, playerPosition);
        SoundManager.Instance.PlaySfx(SFXSound.WDAS004);
        
        //감지 몬스터가 없을 경우 바라보는 방향에 낙뢰 위치 표시 재생 후 return
        if (cols.Length < 1)
        {
            SkillEffect(hitPos, 0,$"{skillNode.skillData.SkillId}_1_Particle" ,skillNode.skillData.SkillEffectPrefab[0]);
            return;
        }
        
        int detected = skillNode.skillData.DetectedCount < cols.Length ? skillNode.skillData.DetectedCount : cols.Length;
        
        for (int j = 0; j < detected; j++)
        {
            SkillEffect(cols[j].transform.position, j, $"{skillNode.skillData.SkillId}_2_Particle", skillNode.skillData.SkillEffectPrefab[0]);
                
            skillActions.Add(new List<Action>());
                
            IEffectReceiver receiver = cols[j].GetComponent<IEffectReceiver>();
                
            skillActions[j].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
            skillActions[j].Add(() => ExecuteStun(receiver, skillNode.skillData.DeBuffDuration));
            skillActions[j].Add(() => RemoveTriggerModuleList());
                
            triggerModules[j].AddCollider(cols[j]);
            interactions[j].ReceiveAction(skillActions[j]);
        }
        
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    } 
}
