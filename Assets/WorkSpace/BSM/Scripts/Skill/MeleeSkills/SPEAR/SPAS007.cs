using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SPAS007 : MeleeSkill
{
    private List<Action> effectAction = new List<Action>();
    private Action skillAction;
    
    public SPAS007(SkillNode skillNode) : base(skillNode)
    {
        leftHash = Animator.StringToHash("SkillMotion_Left_4");
        rightHash = Animator.StringToHash("SkillMotion_Right_4");
        upHash = Animator.StringToHash("SkillMotion_Up_4");
        downHash = Animator.StringToHash("SkillMotion_Down_4");
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        effectAction.Add(() => SkillEffect(playerPosition, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]));
        skillDamage = GetSkillDamage();
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition, overlapSize, 0, monsterLayer);
        
        if (cols.Length > 0)
        {
            effectAction.Add(() => RegisterAction(cols));
        } 
        SoundManager.Instance.PlaySfx(SFXSound.SPAS007);
        ExecuteJumpAttackInPlace(effectAction); 
    }

    /// <summary>
    /// 스킬 모션 이후 취할 행동 등록
    /// </summary>
    /// <param name="cols"></param>
    private void RegisterAction(Collider2D[] cols)
    {
        skillActions.Add(new List<Action>());
        int detectedCount = skillNode.skillData.DetectedCount < cols.Length ? skillNode.skillData.DetectedCount : cols.Length;

        for (int i = 0; i < detectedCount; i++)
        {
            IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
                
            skillActions[0].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
            skillActions[0].Add(() => ExecuteStun(receiver, skillNode.skillData.DeBuffDuration));
            triggerModules[0].AddCollider(cols[i]);
        }
        skillActions[0].Add(() => RemoveTriggerModuleList(0));
        interactions[0].ReceiveAction(skillActions[0]);
    }
    
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
