using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOAS002 : ProjectileSkill
{ 
    public BOAS002(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        SetSkillDamage(skillNode.skillData.SkillDamage);
        skillNode.PlayerSkillReceiver.StartCoroutine(FireEffectInterval(playerPosition, direction));
    }

    /// <summary>
    /// 화살 발사 시 일정 텀을 줌
    /// </summary>
    /// <param name="position">발사가 시작될 위치</param>
    /// <param name="direction">발사될 방향</param>
    /// <returns></returns>
    private IEnumerator FireEffectInterval(Vector2 position, Vector2 direction)
    {
        for (int i = 0; i < skillNode.skillData.SkillHitCount; i++)
        {
            //활 주변에 발사 이펙트 재생
            SurroundEffect(position + direction, skillNode.skillData.SkillEffectPrefab[0], $"{skillNode.skillData.SkillId}_1_Particle");
            SetSurroundPrefabLocalRotation(direction, 180f, 0f, 90f, 270f);
            SetSurroundSortingOrder(direction);
            //화살 풀에서 화살 오브젝트 꺼내옴
            GameObject arrowInstance = ArrowPool.Instance.GetPoolArrow();
            
            Arrow arrow = arrowInstance.GetComponent<Arrow>();
            
            //화살의 발사 위치 및 최대 사거리 설정
            arrow.SetLaunchTransform(position, direction, skillNode.skillData.SkillRange);
            
            //화살의 데미지 설정
            arrow.SetArrowDamage(skillDamage);
            
            yield return WaitCache.GetWait(0.2f);
        } 
    }

    /// <summary>
    /// 현재 캐릭터가 바라보는 방향에 따라 Order layer 설정
    /// </summary>
    /// <param name="dir">캐릭터가 바라보는 방향</param>
    private void SetSurroundSortingOrder(Vector2 dir)
    {
        if (dir.y > 0)
        {
            particleSystemRenderer.sortingOrder = -1;
        }
        else
        {
            particleSystemRenderer.sortingOrder = 0;
        }
    }
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
