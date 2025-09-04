using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAS008 : MeleeSkill
{
    public SSAS008(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SkillEffect(playerPosition + Vector2.up, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        ExecuteDefenseUpSpeedDown(skillNode.skillData.BuffDuration, 0.5f, 2f);
        //TODO: 사용 시 아이콘 하나 띄우기
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}