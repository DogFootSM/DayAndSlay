using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Windows.Speech;

public abstract class MeleeSkill : SkillFactory
{
    private ParticleSystem.MainModule mainModule;
    private Vector2 currentDirection;
    
    protected float skillDamage;
    
    public MeleeSkill(SkillNode skillNode) : base(skillNode)
    {
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
    /// 근접 공격 이펙트
    /// </summary>
    protected void MeleeEffect(Vector2 position, Vector2 direction, string skillId, GameObject skillEffectPrefab)
    {
        GameObject instance = particlePooling.GetSkillPool(skillId, skillEffectPrefab);
        instance.transform.parent = null;
        instance.transform.position = position + direction;

        ParticleSystem particleSystem = instance.GetComponent<ParticleSystem>();
        ParticleStopAction stopAction = instance.GetComponent<ParticleStopAction>();
        stopAction.SkillID = skillId;

        mainModule = particleSystem.main;
        currentDirection = direction;

        instance.SetActive(true);
        particleSystem.Play();
    }

    /// <summary>
    /// 파티클의 StartRotation을 회전
    /// </summary>
    /// <param name="leftDeg">왼쪽 방향일 경우의 회전 값</param>
    /// <param name="rightDeg">오른쪽 방향일 경우의 회전 값 </param>
    /// <param name="downDegY">아래 방향일 경우의 회전 값</param>
    /// <param name="upDegY">윗 방향일 경우의 회전 값</param>
    protected void SetParticleStartRotationFromDeg(float leftDeg, float rightDeg, float downDegY, float upDegY)
    {
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
    protected void KnockBackEffect(Vector2 playerPos, Vector2 playerDir, IEffectReceiver monster)
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
    protected void DotEffect(IEffectReceiver monster, float duration, float tick, float damage)
    {
        monster.ReceiveDot(duration, tick, damage);
    }
    
    /// <summary>
    /// 스턴 효과
    /// </summary>
    /// <param name="monster">감지한 몬스터</param>
    /// <param name="duration">스턴 지속 시간</param>
    protected void StunEffect(IEffectReceiver monster, float duration)
    {
        monster.ReceiveStun(duration);
    }

    /// <summary>
    /// 적 둔화 효과 호출
    /// </summary>
    /// <param name="monster">감지한 몬스터</param>
    /// <param name="duration">둔화 지속 시간</param>
    protected void SlowEffect(IEffectReceiver monster, float duration)
    {
        monster.ReceiveSlow(duration);
    }

    /// <summary>
    /// 쉴드 스킬 호출
    /// </summary>
    /// <param name="castingTime">스킬 사용에 걸리는 시간</param>
    /// <param name="shieldCount">스킬 사용 시 충전할 쉴드 개수</param>
    /// <param name="defenseBoostMultiplier">쉴드 사용 시 증가할 방어력</param>
    /// <param name="duration">스킬 지속 시간</param>
    protected void ShieldEffect(float castingTime, int shieldCount, float defenseBoostMultiplier, float duration)
    {
        skillNode.PlayerSkillReceiver.ReceiveShield(castingTime, shieldCount, defenseBoostMultiplier, duration);
    }

    /// <summary>
    /// 스킬 사용 시 이동 불가 상태 효과 호출
    /// </summary>
    /// <param name="duration">이동 불가할 지속 시간</param>
    protected void ApplyMovementBlock(float duration)
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
    /// 몬스터 방어력 감소 디버프 효과 호출
    /// </summary>
    /// <param name="monster">감지한 몬스터</param>
    /// <param name="duration">디버프 지속 시간</param>
    /// <param name="deBuffPercent">방어력 감소 비율</param>
    protected void ApplyDefenseDeBuff(IEffectReceiver monster, float duration, float deBuffPercent)
    {
        monster.ReceiveDefenseDeBuff(duration, deBuffPercent);
    }

    /// <summary>
    /// 다음 스킬 데미지 버프 적용
    /// </summary>
    /// <param name="multiplier"></param>
    protected void ApplyNextSkillDamageBuff(float multiplier)
    {
        skillNode.PlayerModel.NextSkillDamageMultiplier = multiplier;
        skillNode.PlayerModel.NextSkillBuffActive = true;
    }
    
    /// <summary>
    /// 몬스터에게 데미지 전달
    /// </summary>
    /// <param name="monster">감지한 몬스터</param>
    /// <param name="damage">스킬 데미지</param>
    protected void Hit(IEffectReceiver monster, float damage)
    {
        monster.TakeDamage(damage);
    }
}