using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAS004 : MeleeSkill
{
    public SSAS004(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();

        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        
        Collider2D[] detected = Physics2D.OverlapBoxAll(playerPosition + direction, overlapSize, 0, monsterLayer);
        skillDamage = GetSkillDamage();
        
        if (detected.Length > 0)
        {
            float deBuffDuration = GetDeBuffDurationIncreasePerLevel(5f);
            
            for (int i = 0; i < 3; i++)
            {
                IEffectReceiver monster = detected[i].GetComponent<IEffectReceiver>();
                SkillEffect(detected[i].transform.position + Vector3.up, i, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
                skillActions.Add(new List<Action>());
                skillActions[i].Add(() => ExecuteSlow(monster, deBuffDuration, 0.5f));
                skillActions[i].Add(() => Hit(monster, skillDamage, skillNode.skillData.SkillHitCount));
                skillActions[i].Add(() => RemoveTriggerModuleList(i));
                triggerModules[i].AddCollider(detected[i]);
                interactions[i].ReceiveAction(skillActions[i]); 
            } 
        } 
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType){ }

    public override void Gizmos() { }
}
