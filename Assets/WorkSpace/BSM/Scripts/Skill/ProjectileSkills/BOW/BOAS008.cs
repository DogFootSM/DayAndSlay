using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOAS008 : MeleeSkill
{
    private float moveSpeedPer = 0.25f;
    private float moveSpeedLevelPer = 0.03f;
    
    public BOAS008(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SkillEffect(playerPosition + Vector2.up, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        
        float buffLevelPer = moveSpeedPer + ((skillNode.CurSkillLevel - 1) * moveSpeedLevelPer);
        ExecuteMoveSpeedBuff(skillNode.skillData.BuffDuration, buffLevelPer * skillNode.PlayerModel.PlayerStats.baseMoveSpeed);
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
