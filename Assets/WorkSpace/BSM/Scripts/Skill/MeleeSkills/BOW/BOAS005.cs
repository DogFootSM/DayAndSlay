using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOAS005 : MeleeSkill
{
    private Vector2 hitPos;
    
    public BOAS005(SkillNode skillNode) : base(skillNode)
    {
        leftHash = Animator.StringToHash("SkillMotion_Left_3");
        rightHash = Animator.StringToHash("SkillMotion_Right_3");
        upHash = Animator.StringToHash("SkillMotion_Up_3");
        downHash = Animator.StringToHash("SkillMotion_Down_3");
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SoundManager.Instance.PlaySfx(SFXSound.BOAS005);
        SkillEffect(playerPosition, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        SetDirectionToRotation(direction);

        hitPos = playerPosition + (direction * (skillNode.skillData.SkillRange / 2));
        
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        Collider2D[] cols = Physics2D.OverlapBoxAll(hitPos, overlapSize, 0, monsterLayer);
        Sort.SortMonstersByNearest(cols, playerPosition);
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

        float deBuffLevelPer = skillNode.skillData.SkillAbilityValue + ((skillNode.CurSkillLevel -1) * skillNode.skillData.SkillAbilityFactor);
        int detectedCount = skillNode.skillData.DetectedCount < cols.Length ? skillNode.skillData.DetectedCount : cols.Length;
        
        for (int i = 0; i < detectedCount; i++)
        {
            skillActions.Add(new List<Action>());
            SkillEffect(cols[i].transform.position, i, $"{skillNode.skillData.SkillId}_2_Particle", skillNode.skillData.SkillEffectPrefab[1]);
                
            IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
            skillActions[i].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
            skillActions[i].Add(() => ExecuteAttackDeBuffByMonster(receiver, skillNode.skillData.DeBuffDuration, deBuffLevelPer));
            skillActions[i].Add(() => RemoveTriggerModuleList());
            
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
        UnityEngine.Gizmos.color = Color.red;
        UnityEngine.Gizmos.DrawWireCube(hitPos, overlapSize);
    }
}