using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SPAS001 : MeleeSkill
{
    public SPAS001(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        
        GameObject instance = particlePooling.GetSkillPool($"{skillNode.skillData.SkillId}_2_Particle", skillNode.skillData.SkillEffectPrefab[1]);
        particlePooling.ReturnSkillParticlePool($"{skillNode.skillData.SkillId}_2_Particle", instance);

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            instance.transform.localScale = new Vector2(1.5f, 1f);
        }
        else
        {
            instance.transform.localScale = new Vector2(1f, 1.5f);
        }
        
        MultiEffect(playerPosition, 0, $"{skillNode.skillData.SkillId}_1_Particle" , skillNode.skillData.SkillEffectPrefab[0]);
        ExecuteDash(direction);
        
        
        
        
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
