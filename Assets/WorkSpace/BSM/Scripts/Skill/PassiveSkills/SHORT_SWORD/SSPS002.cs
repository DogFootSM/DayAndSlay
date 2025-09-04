using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSPS002 : PassiveSkill
{
    public SSPS002(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        float factor = skillNode.PlayerModel.PlayerStats.baseStrength * 0.1f;
        skillNode.PlayerModel.UpdateStrengthFactor(factor);

        float criticalPer = skillNode.PlayerModel.PlayerStats.baseCriticalPer + 0.05f;
        float criticalPerLevel = criticalPer + (skillNode.CurSkillLevel * 0.02f);
        skillNode.PlayerModel.UpdateCriticalPerFactor(criticalPerLevel);
    }

    public override void Gizmos()
    {
        
    }

    public override void RevertPassiveEffects()
    {
        float factor = skillNode.PlayerModel.PlayerStats.baseStrength * -0.1f;

        skillNode.PlayerModel.UpdateStrengthFactor(factor);
        skillNode.PlayerModel.UpdateCriticalPerFactor(0);
    }
}
