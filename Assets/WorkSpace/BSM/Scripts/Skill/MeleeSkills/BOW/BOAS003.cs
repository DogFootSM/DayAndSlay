using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOAS003 : MeleeSkill
{
    private int shieldCount = 1;
    
    public BOAS003(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear(); 
        //현재 스킬 레벨 당 지속 시간
        float duration = skillNode.skillData.BuffDuration + ((skillNode.CurSkillLevel - 1) * 5f);
        
        ExecuteCasting(3f);
        ExecuteShield(shieldCount, 0, duration);
        skillNode.PlayerSkillReceiver.StartCoroutine(CastingWaitEffect(playerPosition));
    }

    private IEnumerator CastingWaitEffect(Vector2 effectPosition)
    {
        yield return new WaitUntil(() => !skillNode.PlayerModel.IsCasting);
        SkillEffect(effectPosition, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        instance.transform.parent = skillNode.PlayerModel.transform;
        
        //이펙트 Local 위치 설정
        instance.transform.localPosition = Vector2.zero;
    }
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
