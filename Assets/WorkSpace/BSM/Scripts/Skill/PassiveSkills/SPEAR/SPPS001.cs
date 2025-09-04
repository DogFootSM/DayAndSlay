using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPPS001 : PassiveSkill
{
    private float strengthValueModifier = 0.12f;
    private float criticalValueModifier = 0.02f;
    
    public SPPS001(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;
        
        //기본 힘 능력치에 12% 1회 추가
        float factor = skillNode.PlayerModel.PlayerStats.baseStrength * strengthValueModifier;
        skillNode.PlayerModel.UpdateStrengthFactor(factor);
        
        //기본 크리티컬 능력치에 5% 추가
        float criFactor = 0.05f; 

        //스킬 레벨 당 2%추가
        float criticalPerLevel = criFactor + ((skillNode.CurSkillLevel - 1) * criticalValueModifier);
        
        skillNode.PlayerModel.UpdateCriticalPerFactor(criticalPerLevel);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        float factor = skillNode.PlayerModel.PlayerStats.baseStrength * -strengthValueModifier;
        
        //추가된 힘 스탯 복원
        skillNode.PlayerModel.UpdateStrengthFactor(factor);
        
        //기존 크리티컬 확률로 변경
        skillNode.PlayerModel.UpdateCriticalPerFactor(0);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
