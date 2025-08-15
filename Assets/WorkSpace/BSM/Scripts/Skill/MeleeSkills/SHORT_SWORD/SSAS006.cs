using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAS006 : MeleeSkill
{
    private float deBuffRatio = 0.3f;
    
    public SSAS006(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        skillDamage = GetSkillDamage();

        Collider2D[] monster = Physics2D.OverlapBoxAll(playerPosition + direction, overlapSize, 0, monsterLayer);

        if (monster.Length > 0)
        {
            MeleeEffect(monster[0].transform.position - new Vector3(0, 0.5f), skillNode.skillData.SkillId, skillNode.skillData.SkillEffectPrefab);
            IEffectReceiver receiver = monster[0].GetComponent<IEffectReceiver>();

            float deBuffDuration = GetDeBuffDurationIncreasePerLevel(1);
            
            skillActions.Add(() => ExecuteDefenseDeBuff(receiver, deBuffDuration, deBuffRatio));
            skillActions.Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
            skillActions.Add(RemoveTriggerModuleList);
            
            if (triggerModule.enabled)
            {
                triggerModule.AddCollider(monster[0]);
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

    public override void ApplyPassiveEffects()
    {
    }

    public override void Gizmos()
    {
    }
}