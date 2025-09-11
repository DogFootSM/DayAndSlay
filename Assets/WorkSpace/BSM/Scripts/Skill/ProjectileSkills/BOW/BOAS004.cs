using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOAS004 : ProjectileSkill
{
    private Coroutine delayFireCo;
    private Vector2 target = new Vector2();
    private float maxDistance = 0f;
    private int effectIndex = 0;
    
    public BOAS004(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        //이펙트 재생
        SingleEffect(playerPosition + direction / 2, skillNode.skillData.SkillEffectPrefab[0], $"{skillNode.skillData.SkillId}_1_Particle", effectIndex++);
        
        //이펙트 방향에 따라 회전
        SetSurroundPrefabLocalRotation(direction, 180f, 0, 90f, 270f);
        
        //스킬 데미지 설정
        SetSkillDamage(skillNode.skillData.SkillDamage);
        
        if (direction.y > 0)
        {
            particleSystemRenderer[0].sortingOrder = -1;
        }
        else
        {
            particleSystemRenderer[0].sortingOrder = 50;
        }

        if (delayFireCo == null)
        {
            delayFireCo = skillNode.PlayerSkillReceiver.StartCoroutine(DelayFireRoutine(playerPosition, direction));
        } 
    }

    private IEnumerator DelayFireRoutine(Vector2 playerPosition, Vector2 direction)
    {
        yield return WaitCache.GetWait(0.9f);
        
        //이펙트 재생
        SingleEffect(playerPosition + direction, skillNode.skillData.SkillEffectPrefab[1], $"{skillNode.skillData.SkillId}_2_Particle", effectIndex);
        
        //해당 스킬 발사체 파티클 데이터 설정
        surroundEffectInstance.GetComponent<ActionBOAS004>().SetSkillData(3, skillNode.skillData.SkillHitCount, skillDamage);
        
        //일직선으로 날아갈 발사체 제한 거리 설정
        surroundInteraction[effectIndex].LinearProjectile(0, direction, skillNode.skillData.SkillRange);
        
        if (delayFireCo != null)
        {
            skillNode.PlayerSkillReceiver.StopCoroutine(delayFireCo);
            delayFireCo = null;
        }
    }
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
