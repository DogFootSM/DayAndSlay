using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BOAS002 : ProjectileSkill
{
    public BOAS002(SkillNode skillNode) : base(skillNode)
    {
        leftHash = Animator.StringToHash("SkillMotion_Left_2");
        rightHash = Animator.StringToHash("SkillMotion_Right_2");
        upHash = Animator.StringToHash("SkillMotion_Up_2");
        downHash = Animator.StringToHash("SkillMotion_Down_2");
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
        float slowRatio = skillNode.skillData.SkillAbilityValue +
                          ((skillNode.CurSkillLevel - 1) * skillNode.skillData.SkillAbilityFactor);
        
        for (int i = 0; i < skillNode.skillData.SkillHitCount; i++)
        {
            //활 주변에 발사 이펙트 재생
            SingleEffect(position + direction, skillNode.skillData.SkillEffectPrefab[0], $"{skillNode.skillData.SkillId}_1_Particle", i);
            SetSurroundPrefabLocalRotation(direction, 180f, 0f, 90f, 270f);
            SetSurroundSortingOrder(direction, i);
            //화살 풀에서 화살 오브젝트 꺼내옴
            GameObject arrowInstance = ArrowPool.Instance.GetPoolArrow();
            
            Arrow arrow = arrowInstance.GetComponent<Arrow>();
            
            //화살의 발사 위치 및 최대 사거리 설정
            arrow.SetLaunchTransform(position, direction, skillNode.skillData.SkillRange);
            
            //화살의 데미지 설정
            arrow.SetArrowDamage(skillDamage);
            
            //화살 슬로우 스킬 적용, 현재 레벨에 따른 0.05% 추가 수치 적용
            arrow.SetSlowSkill(true, skillNode.skillData.DeBuffDuration, slowRatio);
            yield return WaitCache.GetWait(0.2f);
        } 
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
