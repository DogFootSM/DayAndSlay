using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDAS001 : ProjectileSkill
{ 
    private int effectIndex = 0;
    
    public WDAS001(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        SetSkillDamage(skillNode.skillData.SkillDamage);
        ExecuteCasting(skillNode.skillData.SkillCastingTime);
        
        skillNode.PlayerSkillReceiver.StartCoroutine(WaitCastingRoutine(playerPosition, direction));
    }

    private IEnumerator WaitCastingRoutine(Vector2 position, Vector2 direction)
    {
        yield return new WaitUntil(() => !skillNode.PlayerModel.IsCasting);
        Fire(position, direction);
    }
    
    /// <summary>
    /// 미사일 발사
    /// </summary>
    /// <param name="position">발사가 시작될 위치</param>
    /// <param name="direction">발사될 방향</param>
    /// <returns></returns>
    private void Fire(Vector2 position, Vector2 direction)
    {
        skillNode.skillData.SkillEffectPrefab[0].GetComponent<MagicMissile>().SetData(skillNode.skillData.SkillHitCount, skillNode.skillData.SkillDamage);

        //몸 주변에 발사 이펙트 재생
        SingleEffect(position + direction, skillNode.skillData.SkillEffectPrefab[0], $"{skillNode.skillData.SkillId}_1_Particle", 0);
        
        //히트 이펙트 풀에 반납할 ID 설정
        surroundInteraction[effectIndex].SetHitEffectId($"{skillNode.skillData.SkillId}_2_Particle");
        
        //바라보는 방향으로 발사
        surroundInteraction[effectIndex].LinearProjectile(0, direction, skillNode.skillData.SkillRange, 5f);
    } 
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
