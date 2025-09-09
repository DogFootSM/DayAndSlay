using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSPS003 : PassiveSkill
{
    private float damageReflectValueModifier = 0.08f;
    private float damageReflectLevelModifier = 0.03f;
    
    public SSPS003(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        skillNode.PlayerModel.PlayerStats.DamageReflectRate = damageReflectValueModifier + ((skillNode.CurSkillLevel - 1) * damageReflectLevelModifier);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers(); 
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        skillNode.PlayerModel.PlayerStats.DamageReflectRate = 0;
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
