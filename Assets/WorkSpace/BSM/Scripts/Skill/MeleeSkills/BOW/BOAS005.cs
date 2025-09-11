using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOAS005 : MeleeSkill
{
    public BOAS005(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SkillEffect(playerPosition, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        SetDirectionToRotation(direction);
        
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition + direction, overlapSize, 0, monsterLayer);
        skillDamage = GetSkillDamage();
        ListClear();
        
        if (cols.Length > 0)
        {
            skillNode.PlayerSkillReceiver.StartCoroutine(DelayEffectRoutine(cols));
        } 
    }

    private IEnumerator DelayEffectRoutine(Collider2D[] cols)
    {
        yield return WaitCache.GetWait(1f);

        float deBuffLevelPer = 0.3f + ((skillNode.CurSkillLevel -1) * 0.05f);
        
        for (int i = 0; i < 3; i++)
        {
            skillActions.Add(new List<Action>());
            SkillEffect(cols[i].transform.position, i, $"{skillNode.skillData.SkillId}_2_Particle", skillNode.skillData.SkillEffectPrefab[1]);
                
            IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
            skillActions[i].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
            skillActions[i].Add(() => ExecuteAttackDeBuffByMonster(receiver, skillNode.skillData.DeBuffDuration, deBuffLevelPer));
            skillActions[i].Add(() => RemoveTriggerModuleList(0));
            
            triggerModules[i].AddCollider(cols[i]);
            interactions[i].ReceiveAction(skillActions[i]);
        } 
    }
    
    private void SetDirectionToRotation(Vector2 direction)
    {
        if(direction.x > 0) instance.transform.rotation = Quaternion.Euler(0f, 0f, 25f);
        if(direction.x < 0) instance.transform.rotation = Quaternion.Euler(0f, 0f, 155f);
        if(direction.y > 0) instance.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        if(direction.y < 0) instance.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
    }
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}