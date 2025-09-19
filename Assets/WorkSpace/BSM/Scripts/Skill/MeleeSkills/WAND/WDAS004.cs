using System;
using System.Collections;
using System.Collections.Generic;
using ModestTree.Util;
using UnityEngine;

public class WDAS004 : MeleeSkill
{
    private Action action;
    private Coroutine castingCo;

    private int thunderCount = 2;
    
    public WDAS004(SkillNode skillNode) : base(skillNode)
    {
    } 
    
    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        skillDamage = GetSkillDamage();
        ExecuteCasting(skillNode.skillData.SkillCastingTime);

        action = () => ExecutePostCastAction(direction, playerPosition);
        castingCo = skillNode.PlayerSkillReceiver.StartCoroutine(WaitCastingRoutine(action)); 
    }

    /// <summary>
    /// 캐스팅 이후 수행할 동작
    /// </summary>
    /// <param name="direction">캐릭터가 스킬 사용시 바라본 방향</param>
    /// <param name="playerPosition">캐릭터가 스킬을 사용한 위치</param>
    private void ExecutePostCastAction(Vector2 direction, Vector2 playerPosition)
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition + (direction * skillNode.skillData.SkillRange),
            overlapSize, 0, monsterLayer);

        //감지 몬스터가 없을 경우 바라보는 방향에 낙뢰 위치 표시 재생 후 return
        if (cols.Length < 1)
        {
            SkillEffect(playerPosition + (direction * skillNode.skillData.SkillRange), 0,$"{skillNode.skillData.SkillId}_1_Particle" ,skillNode.skillData.SkillEffectPrefab[0]);
            return;
        }
        
        for (int i = 0; i < cols.Length; i++)
        {
            SkillEffect(cols[i].transform.position + new Vector3(0, -0.5f), i,$"{skillNode.skillData.SkillId}_1_Particle" ,skillNode.skillData.SkillEffectPrefab[0]);
        }

        skillNode.PlayerSkillReceiver.StartCoroutine(ThunderEffectRoutine(cols));
    }

    /// <summary>
    /// 번개 이펙트 코루틴
    /// </summary> 
    /// <returns></returns>
    private IEnumerator ThunderEffectRoutine(Collider2D[] cols)
    { 
        for (int i = 0; i < thunderCount; i++)
        {
            ListClear();
            
            for (int j = 0; j < cols.Length; j++)
            {
                SkillEffect(cols[j].transform.position, j, $"{skillNode.skillData.SkillId}_2_Particle", skillNode.skillData.SkillEffectPrefab[1]);
                
                skillActions.Add(new List<Action>());
                
                IEffectReceiver receiver = cols[j].GetComponent<IEffectReceiver>();
                
                skillActions[j].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
                skillActions[j].Add(() => ExecuteStun(receiver, skillNode.skillData.DeBuffDuration));
                skillActions[j].Add(() => RemoveTriggerModuleList(0));
                
                triggerModules[j].AddCollider(cols[j]);
                interactions[j].ReceiveAction(skillActions[j]);
            }

            yield return WaitCache.GetWait(0.6f);
        } 
    }
    
     
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    } 
}
