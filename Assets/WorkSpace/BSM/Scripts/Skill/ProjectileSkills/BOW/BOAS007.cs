using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOAS007 : ProjectileSkill
{
    public BOAS007(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        SetSkillDamage(skillNode.skillData.SkillDamage);
        GameObject chombInstance = particlePooling.GetSkillPool($"{skillNode.skillData.SkillId}_Chomb", skillNode.skillData.SkillEffectPrefab[0]);
        chombInstance.transform.position = playerPosition;
        chombInstance.SetActive(true);
        
        Chomb chomb = chombInstance.GetComponent<Chomb>();
        chomb.SetSkillData(skillDamage, $"{skillNode.skillData.SkillId}_Chomb");
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
