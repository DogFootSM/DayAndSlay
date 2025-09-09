using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSPS002 : PassiveSkill
{
    private float criticalLevelModifier = 0.02f;
    
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

        float criticalPer = 0.05f;
        float criticalPerLevel = criticalPer + ((skillNode.CurSkillLevel - 1) * criticalLevelModifier);
        skillNode.PlayerModel.UpdateCriticalPerFactor(criticalPerLevel);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
        
    }

    public override void RevertPassiveEffects()
    {
        float factor = skillNode.PlayerModel.PlayerStats.baseStrength * -0.1f;

        skillNode.PlayerModel.UpdateStrengthFactor(factor);
        skillNode.PlayerModel.UpdateCriticalPerFactor(0);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
