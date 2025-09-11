using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDAS001 : ProjectileSkill
{ 
    private Coroutine delayFireCo;
    private Vector2 target = new Vector2();
    private float maxDistance = 0f;
    private int effectIndex = 0;
    
    private MagicMissile missile;
    
    public WDAS001(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        SetSkillDamage(skillNode.skillData.SkillDamage);
        ExecuteCasting(skillNode.skillData.SkillCastingTime);
        Fire(playerPosition, direction);

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
        
        
        //회전 방향 조절
        SetSurroundPrefabLocalRotation(direction, 180f, 0f, 90f, 270f);

        //SortingOrder 조절
        SetSurroundSortingOrder(direction, 0);

        //요놈도 사거리제한
        surroundInteraction[effectIndex].LinearProjectile(0, direction, skillNode.skillData.SkillRange);
    }
    

    /// <summary>
    /// 현재 캐릭터가 바라보는 방향에 따라 Order layer 설정
    /// </summary>
    /// <param name="dir">캐릭터가 바라보는 방향</param>
    private void SetSurroundSortingOrder(Vector2 dir, int index)
    {
        particleSystemRenderer[index].sortingOrder = dir.y > 0 ? -1 : 0; 
    }
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
