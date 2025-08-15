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
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        
        Collider2D[] detected = Physics2D.OverlapBoxAll(playerPosition + direction, overlapSize, 0, monsterLayer);
        skillDamage = GetSkillDamage();

        if (detected.Length > 0)
        {
            IEffectReceiver monster = detected[0].GetComponent<IEffectReceiver>();
            
            MeleeEffect(detected[0].transform.position + Vector3.up, skillNode.skillData.SkillId, skillNode.skillData.SkillEffectPrefab);
            
            skillActions.Add(() => SlowEffect(monster, skillNode.skillData.DeBuffDuration, 0.5f));
            skillActions.Add(() => Hit(monster, skillDamage, skillNode.skillData.SkillHitCount));
            skillActions.Add(RemoveTriggerModuleList);
            
            if (triggerModule.enabled)
            {
                triggerModule.AddCollider(detected[0]);
                interaction.ReceiveAction(skillActions);
            } 
        } 
    }

    public override void ApplyPassiveEffects(){ }

    public override void Gizmos() { }
}
