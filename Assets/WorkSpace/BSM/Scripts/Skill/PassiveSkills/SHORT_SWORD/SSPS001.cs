using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSPS001 : PassiveSkill
{
    public SSPS001(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    { 
    }

    public override void ApplyPassiveEffects()
    {
        PassiveEffect();
        Debug.Log($"{skillNode.skillData.SkillName} 패시브 스킬");
        
        //TODO: 패시브 스킬 공식 수정 필요
        skillNode.PlayerModel.PlayerStats.CriticalDamage +=
            (1 + (skillNode.skillData.SkillCooldown * skillNode.CurSkillLevel));
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
        throw new System.NotImplementedException();
    }
}
