using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOAS007 : ProjectileSkill
{
    public BOAS007(SkillNode skillNode) : base(skillNode)
    {
        leftHash = Animator.StringToHash("SkillMotion_Left_5");
        rightHash = Animator.StringToHash("SkillMotion_Right_5");
        upHash = Animator.StringToHash("SkillMotion_Up_5");
        downHash = Animator.StringToHash("SkillMotion_Down_5");
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        SetSkillDamage(skillNode.skillData.SkillDamage);
        GameObject chombInstance = particlePooling.GetSkillPool($"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        chombInstance.transform.position = playerPosition;
        
        Chomb chomb = chombInstance.GetComponent<Chomb>();
        chomb.SetSkillData(skillNode.skillData.DetectedCount, skillDamage, $"{skillNode.skillData.SkillId}_1_Particle");
        chombInstance.SetActive(true);
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
