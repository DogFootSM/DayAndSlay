using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SSAS006 : MeleeSkill
{
    private Vector2 hitPos;
    
    public SSAS006(SkillNode skillNode) : base(skillNode)
    {
        leftHash = Animator.StringToHash("SkillMotion_Left_6");
        rightHash = Animator.StringToHash("SkillMotion_Right_6");
        upHash = Animator.StringToHash("SkillMotion_Up_6");
        downHash = Animator.StringToHash("SkillMotion_Down_6");
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        skillDamage = GetSkillDamage();
        hitPos = playerPosition + (direction * (skillNode.skillData.SkillRange / 2));
        
        Collider2D[] cols = Physics2D.OverlapBoxAll(hitPos, overlapSize, 0, monsterLayer);
        Sort.SortMonstersByNearest(cols, playerPosition);

        float defenseDeBuffFactor = skillNode.skillData.SkillAbilityValue +
                                    ((skillNode.CurSkillLevel - 1) * skillNode.skillData.SkillAbilityFactor);
        
        SoundManager.Instance.PlaySfx(SFXSound.SSAS006);
        
        if (cols.Length > 0)
        {
            skillActions.Add(new List<Action>());
            int detected = skillNode.skillData.DetectedCount < cols.Length ? skillNode.skillData.DetectedCount : cols.Length;
            
            for (int i = 0; i < detected; i++)
            {
                SkillEffect(cols[i].transform.position - new Vector3(0, 0.5f), i, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[i]);
                
                IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
                skillActions[i].Add(() => ExecuteDefenseDeBuff(receiver, skillNode.skillData.DeBuffDuration, defenseDeBuffFactor));
                skillActions[i].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
                triggerModules[i].AddCollider(cols[0]);
            }

            skillActions[0].Add(() => RemoveTriggerModuleList());
            interactions[0].ReceiveAction(skillActions[0]);
        }
        else
        {
            Vector2 offsetWidth = new Vector2();
            Vector2 offsetHeight = new Vector2();
            
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                offsetWidth = new Vector2(skillNode.skillData.SkillRange, 0);
                offsetHeight = playerPosition + (Vector2.down / 2);

            }
            else
            {
                offsetWidth = new Vector2(0, skillNode.skillData.SkillRange);
                offsetHeight = playerPosition;
            }
            
            SkillEffect((offsetHeight + (direction * offsetWidth)), 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        }
        
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
        UnityEngine.Gizmos.color = Color.red;
        UnityEngine.Gizmos.DrawWireCube(hitPos, overlapSize);

    }
}