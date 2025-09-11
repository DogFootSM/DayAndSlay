using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSPS002 : PassiveSkill
{
    private float criticalLevelFactor = 0.02f;
    private float criticalbaseFactor = 0.05f;
    
    private float baseStrengthFactor = 0.1f;
    
    public SSPS002(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;
        StrengthBuff(baseStrengthFactor);
        CriticalRateBuff(criticalbaseFactor, criticalLevelFactor);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
        
    }

    public override void RevertPassiveEffects()
    {
        skillNode.PlayerModel.UpdateStrengthFactor(0);
        skillNode.PlayerModel.UpdateCriticalPerFactor(0);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
