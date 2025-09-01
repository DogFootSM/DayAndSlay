using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Windows.Speech;

public abstract class MeleeSkill : SkillFactory
{

    protected Vector2 currentDirection;
    protected ParticleSystem.MainModule mainModule;

    protected GameObject instance;
    protected List<List<Action>> multiActions = new List<List<Action>>();
    protected List<ParticleSystem.MainModule> mainModules = new List<ParticleSystem.MainModule>();
    protected List<ParticleSystem.TriggerModule> triggerModules = new List<ParticleSystem.TriggerModule>();
    protected List<ParticleInteraction> interactions = new List<ParticleInteraction>();
    protected ParticleSystemRenderer particleSystemRenderer;
    
    protected float skillDamage;
    protected float leftDeg; 
    protected float rightDeg;
    protected float downDeg;
    protected float upDeg;
    
    public MeleeSkill(SkillNode skillNode) : base(skillNode)
    {
    }

    protected void ListClear()
    {
        multiActions.Clear();
        mainModules.Clear();
        triggerModules.Clear();
        interactions.Clear();
    }
    
    protected void SetOverlapSize(Vector2 direction, float skillRange)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            //TODO: 감지 모양 및 크기는 추후 수정
            overlapSize = new Vector2(skillRange, 1f);
        }
        else
        {
            overlapSize = new Vector2(1f, skillRange);
        }
    }

    protected void SetOverlapSize(float range)
    {
        overlapSize = new Vector2(range, range);
    }
    
    /// <summary>
    /// 현재 스킬의 데미지 반환
    /// </summary>
    /// <returns></returns>
    protected float GetSkillDamage()
    {
        //다음 스킬의 데미지 증가 버프가 걸려있는 상태
        if (skillNode.PlayerModel.NextSkillBuffActive)
        {
            skillNode.PlayerModel.NextSkillBuffActive = false;
            return skillNode.skillData.SkillDamage * skillNode.CurSkillLevel * skillNode.PlayerModel.NextSkillDamageMultiplier;
        } 
        
        return skillNode.skillData.SkillDamage * skillNode.CurSkillLevel;
    }

    /// <summary>
    /// 레벨당 디버프 지속 시간 증가 계산
    /// </summary>
    /// <returns></returns>
    protected float GetDeBuffDurationIncreasePerLevel(float increaseValue)
    {
        return skillNode.skillData.DeBuffDuration + (skillNode.CurSkillLevel * increaseValue);
    }
    
    /// <summary>
    /// 근접 공격 이펙트
    /// </summary>
    protected void MultiEffect(Vector2 position, int index, string effectId, GameObject skillEffectPrefab)
    { 
        instance = particlePooling.GetSkillPool(effectId, skillEffectPrefab);
        instance.transform.parent = null;
        instance.transform.position = position;

        ParticleSystem particleSystem = instance.GetComponent<ParticleSystem>();
        interactions.Add(instance.GetComponent<ParticleInteraction>());
        interactions[index].EffectId = effectId;
        particleSystemRenderer = instance.GetComponent<ParticleSystemRenderer>();
        
        mainModules.Add(particleSystem.main); 
        triggerModules.Add(particleSystem.trigger);
        instance.SetActive(true);
        particleSystem.Play();
    }

    protected void SetParticleLocalScale(Vector3 widthScale, Vector3 heightScale)
    {
        if (Mathf.Abs(currentDirection.x) > Mathf.Abs(currentDirection.y))
        {
            instance.transform.localScale = widthScale;
        }
        else
        {
            instance.transform.localScale = heightScale;
        }
        
    }
    
    /// <summary>
    /// 파티클 오브젝트 Flip 설정
    /// </summary>
    /// <param name="flip"></param>
    protected void SetParticleRendererFlip(Vector3 flip)
    {
        particleSystemRenderer.flip = flip;
    }
    
    /// <summary>
    /// 파티클의 StartRotation을 회전
    /// </summary>
    /// <param name="leftDeg">왼쪽 방향일 경우의 회전 값</param>
    /// <param name="rightDeg">오른쪽 방향일 경우의 회전 값 </param>
    /// <param name="downDegY">아래 방향일 경우의 회전 값</param>
    /// <param name="upDegY">윗 방향일 경우의 회전 값</param>
    protected void SetParticleStartRotationFromDeg(int index, Vector2 dir, float leftDeg, float rightDeg, float downDegY, float upDegY)
    {
        currentDirection = dir;
        mainModule = mainModules[index];
        
        if (currentDirection.x < 0) mainModule.startRotationZ = Mathf.Deg2Rad * rightDeg;
        if (currentDirection.x > 0) mainModule.startRotationZ = Mathf.Deg2Rad * leftDeg;
        if (currentDirection.y < 0) mainModule.startRotationZ = Mathf.Deg2Rad * upDegY;
        if (currentDirection.y > 0) mainModule.startRotationZ = Mathf.Deg2Rad * downDegY;
    }
 
    /// <summary>
    /// 넉백 효과
    /// </summary>
    /// <param name="playerPos">현재 캐릭터의 위치</param>
    /// <param name="playerDir">캐릭터가 공격한 방향</param>
    /// <param name="monster">감지한 몬스터</param>
    protected void ExecuteKnockBack(Vector2 playerPos, Vector2 playerDir, IEffectReceiver monster)
    {
        monster.ReceiveKnockBack(playerPos, playerDir);
    }


    /// <summary>
    /// 도트데미지 (출혈, 화상 등) 효과
    /// </summary>
    /// <param name="monster">감지한 몬스터</param>
    /// <param name="duration">지속 시간</param>
    /// <param name="tick">데미지를 가할 시간 간격</param>
    /// <param name="damage">초당 데미지</param>
    protected void ExecuteDot(IEffectReceiver monster, float duration, float tick, float damage)
    {
        monster.ReceiveDot(duration, tick, damage);
    }
    
    /// <summary>
    /// 스턴 효과
    /// </summary>
    /// <param name="monster">감지한 몬스터</param>
    /// <param name="duration">스턴 지속 시간</param>
    protected void ExecuteStun(IEffectReceiver monster, float duration)
    {
        monster.ReceiveStun(duration);
    }
    
    /// <summary>
    /// 대쉬 종료 후 몬스터 스턴 효과
    /// </summary>
    /// <param name="receivers">감지한 몬스터</param>
    /// <param name="duration">스턴 지속 시간</param>
    protected void ExecuteDashStun(IEffectReceiver receivers, float duration)
    {
        skillNode.PlayerSkillReceiver.ReceiveDashStun(receivers, duration);
    }
    
    /// <summary>
    /// 적 둔화 효과 호출
    /// </summary>
    /// <param name="monster">감지한 몬스터</param>
    /// <param name="duration">둔화 지속 시간</param>
    /// <param name="ratio">둔화 효과 적용 비율</param>
    protected void ExecuteSlow(IEffectReceiver monster, float duration, float ratio)
    {
        monster.ReceiveSlow(duration, ratio);
    }

    /// <summary>
    /// 쉴드 스킬 호출
    /// </summary>
    /// <param name="castingTime">스킬 사용에 걸리는 시간</param>
    /// <param name="shieldCount">스킬 사용 시 충전할 쉴드 개수</param>
    /// <param name="defenseBoostMultiplier">쉴드 사용 시 증가할 방어력</param>
    /// <param name="duration">스킬 지속 시간</param>
    protected void ExecuteShield(float castingTime, int shieldCount, float defenseBoostMultiplier, float duration)
    {
        skillNode.PlayerSkillReceiver.ReceiveShield(castingTime, shieldCount, defenseBoostMultiplier, duration);
    }

    /// <summary>
    /// 스킬 사용 시 이동 불가 상태 효과 호출
    /// </summary>
    /// <param name="duration">이동 불가할 지속 시간</param>
    protected void ExecuteMovementBlock(float duration)
    {
        skillNode.PlayerSkillReceiver.ReceiveMovementBlock(duration);
    }

    /// <summary>
    /// 반격 효과 호출
    /// </summary>
    protected void CounterWhileImmobile()
    {
        skillNode.PlayerSkillReceiver.ReceiveCounterWhileImmobile();
    }

    /// <summary>
    /// 이동속도 증가 버프 호출
    /// </summary>
    /// <param name="duration">버프 지속 시간</param>
    /// <param name="ratio">이동 속도 증가 비율</param>
    protected void ExecuteMoveSpeedBuff(float duration, float ratio)
    {
        skillNode.PlayerSkillReceiver.ReceiveMoveSpeedBuff(duration, ratio);
    }

    /// <summary>
    /// 대상 위치 순간이동 기능 호출
    /// </summary>
    /// <param name="target">이동할 타겟 위치</param>
    protected void ExecuteBlinkToMarkedTarget(Collider2D target)
    {
        skillNode.PlayerSkillReceiver.ReceiveBlinkToMarkedTarget(target);
    }
    
    /// <summary>
    /// 몬스터 방어력 감소 디버프 효과 호출
    /// </summary>
    /// <param name="monster">감지한 몬스터</param>
    /// <param name="duration">디버프 지속 시간</param>
    /// <param name="deBuffPercent">방어력 감소 비율</param>
    protected void ExecuteDefenseDeBuff(IEffectReceiver monster, float duration, float deBuffPercent)
    {
        monster.ReceiveDefenseDeBuff(duration, deBuffPercent);
    }

    /// <summary>
    /// 다음 스킬 데미지 버프 적용
    /// </summary>
    /// <param name="multiplier"></param>
    protected void ExecuteNextSkillDamageBuff(float multiplier)
    {
        skillNode.PlayerModel.NextSkillDamageMultiplier = multiplier;
        skillNode.PlayerModel.NextSkillBuffActive = true;
    }

    /// <summary>
    /// 캐릭터 대쉬 사용
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="playerPos"></param>
    /// <param name="overlapSize"></param>
    protected void ExecuteDash(Vector2 dir)
    {
        skillNode.PlayerSkillReceiver.ReceiveDash(dir);
    }

    /// <summary>
    /// 파티클 트리거 리스트 감지된 콜라이더 제거
    /// </summary>
    protected void RemoveTriggerModuleList(int triggerIndex)
    {  
        for (int i = 0; i < triggerModules.Count; i++)
        {
            for (int j = triggerModules[i].colliderCount -1; j >= 0; j--)
            {
                triggerModules[i].RemoveCollider(triggerModules[i].GetCollider(j));
            } 
        } 
    }
    
    /// <summary>
    /// 공격력 증가 방어력 감소 효과 호출
    /// </summary>
    /// <param name="duration">스킬 지속 시간</param>
    /// <param name="defenseDecrease">방어력 감소값</param>
    /// <param name="attackIncrease">공격력 증가값</param>
    protected void ExecuteAttackUpDefenseDown(float duration, float defenseDecrease, float attackIncrease)
    {
        skillNode.PlayerSkillReceiver.ReceiveAttackUpDefenseDown(duration, defenseDecrease, attackIncrease);
    }

    /// <summary>
    /// 방어력 감소, 이동속도 증가 효과 호출
    /// </summary>
    /// <param name="duration">스킬 지속 시간</param>
    /// <param name="speedDecrease">이동속도 감소값</param>
    /// <param name="defenseIncrease">방어력 증가값</param>
    protected void ExecuteDefenseUpSpeedDown(float duration, float speedDecrease, float defenseIncrease)
    {
        skillNode.PlayerSkillReceiver.ReceiveDefenseUpSpeedDown(duration, speedDecrease, defenseIncrease);
    }

    /// <summary>
    /// 데미지 감소 적용 효과 호출
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="damageReduction"></param>
    protected void ExecuteDamageReduction(float duration, float damageReduction)
    {
        skillNode.PlayerSkillReceiver.ReceiveDamageReduction(duration, damageReduction);
    }

    /// <summary>
    /// 초당 체력 회복 스킬 호출
    /// </summary>
    /// <param name="duration">체력 회복 지속 시간</param>
    /// <param name="healthRegen">초당 회복할 체력 수치</param>
    protected void ExecuteHealthRegenTick(float duration, float healthRegen)
    {
        skillNode.PlayerSkillReceiver.ReceiveHealthRegenTick(duration, healthRegen);
    }
    
    /// <summary>
    /// 몬스터에게 데미지 전달
    /// </summary>
    /// <param name="monster">감지한 몬스터</param>
    /// <param name="damage">스킬 데미지</param>
    /// <param name="hitCount">몬스터 타격 횟수</param>
    protected void Hit(IEffectReceiver monster, float damage, int hitCount)
    {
        for (int i = 0; i < hitCount; i++)
        {
            monster.TakeDamage(damage);
        } 
    }
}