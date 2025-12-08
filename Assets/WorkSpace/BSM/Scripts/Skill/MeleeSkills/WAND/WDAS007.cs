using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDAS007 : MeleeSkill
{
    private Action action;
    private Coroutine castingCo;
    
    public WDAS007(SkillNode skillNode) : base(skillNode)
    {
        leftHash = Animator.StringToHash("SkillMotion_Left_3");
        rightHash = Animator.StringToHash("SkillMotion_Right_3");
        upHash = Animator.StringToHash("SkillMotion_Up_3");
        downHash = Animator.StringToHash("SkillMotion_Down_3");
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        skillDamage = GetSkillDamage();
        ExecuteCasting(skillNode.skillData.SkillCastingTime);
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        
        action = () => ExecutePostCastAction(direction, playerPosition);
        castingCo = skillNode.PlayerSkillReceiver.StartCoroutine(WaitCastingRoutine(action));
    }

    private void ExecutePostCastAction(Vector2 direction, Vector2 playerPosition)
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition + (direction * (skillNode.skillData.SkillRange / 2)), overlapSize, 0, monsterLayer);
        Sort.SortMonstersByNearest(cols, playerPosition);
        float cx = 0;
        float cy = 0;
        
        SoundManager.Instance.PlaySfx(SFXSound.WDAS007);
        
        if (cols.Length > 0)
        {
            int detected = skillNode.skillData.DetectedCount < cols.Length ? skillNode.skillData.DetectedCount : cols.Length;
            
            for (int i = 0; i < detected; i++)
            {
                cx += cols[i].transform.position.x;
                cy += cols[i].transform.position.y;
            }

            Vector2 cVector = new Vector2(cx / cols.Length, cy / cols.Length);
            SkillEffect(cVector, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
            skillActions.Add(new List<Action>());

            for (int i = 0; i < detected; i++)
            {
                IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
                
                skillActions[0].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
                skillActions[0].Add(() => ExecuteStun(receiver, skillNode.skillData.DeBuffDuration));
                triggerModules[0].AddCollider(cols[i]);
            }
            
            skillActions[0].Add(() => RemoveTriggerModuleList(0));
            interactions[0].ReceiveAction(skillActions[0]);
        }
        else
        {
            //감지된 적이 없으면 바라본 방향에 이펙트 재생
            SkillEffect(playerPosition + (direction * (skillNode.skillData.SkillRange / 2)), 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        }

    }
 
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
