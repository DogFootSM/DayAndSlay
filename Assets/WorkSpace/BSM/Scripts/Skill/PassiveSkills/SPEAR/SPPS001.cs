using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPPS001 : PassiveSkill
{
    private float baseStrengthFactor = 0.12f;
    
    private float criticalLevelFactor = 0.02f;
    private float criticalBaseFactor = 0.05f;
    
    public SPPS001(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;
        StrengthBuff(baseStrengthFactor);
        CriticalRateBuff(criticalBaseFactor, criticalLevelFactor);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        //추가된 힘 스탯 복원
        skillNode.PlayerModel.UpdateStrengthFactor(0);
        
        //기존 크리티컬 확률로 변경
        skillNode.PlayerModel.UpdateCriticalPerFactor(0);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
