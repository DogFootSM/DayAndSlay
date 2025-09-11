using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOAS009 : ProjectileSkill
{
    public BOAS009(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        SetSkillDamage(skillNode.skillData.SkillDamage);
        
        GameObject toadstoolInstance = particlePooling.GetSkillPool($"{skillNode.skillData.SkillId}_1_Particle",
            skillNode.skillData.SkillEffectPrefab[0]);
        toadstoolInstance.SetActive(true);
        toadstoolInstance.transform.position = playerPosition;
        
        Toadstool toadstool = toadstoolInstance.GetComponent<Toadstool>();
        toadstool.SetSkillData($"{skillNode.skillData.SkillId}_1_Particle", skillDamage, skillNode.skillData.SkillEffectPrefab[1], $"{skillNode.skillData.SkillId}_2_Particle", skillNode.skillData.DeBuffDuration);
        toadstool.SetTargetLocation(direction, 3f);
        
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
