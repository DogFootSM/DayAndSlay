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
        SkillEffect(playerPosition + direction, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
