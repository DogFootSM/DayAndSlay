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
            
            float deBuffDuration = GetDeBuffDurationIncreasePerLevel(5f);
            skillActions.Add(() => ExecuteSlow(monster, deBuffDuration, 0.5f));
            skillActions.Add(() => Hit(monster, skillDamage, skillNode.skillData.SkillHitCount));
            skillActions.Add(RemoveTriggerModuleList);
            
            if (triggerModule.enabled)
            {
                triggerModule.AddCollider(detected[0]);
                interaction.ReceiveAction(skillActions);
            } 
        }
        else
        {
            Vector2 offset = new Vector2();
            
            if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) offset = new Vector2(skillNode.skillData.SkillRange, 0);
            else offset = new Vector2(0, skillNode.skillData.SkillRange);
            
            MeleeEffect((playerPosition + (direction * offset)), skillNode.skillData.SkillId, skillNode.skillData.SkillEffectPrefab);
        }
    }

    public override void ApplyPassiveEffects(){ }

    public override void Gizmos() { }
}
