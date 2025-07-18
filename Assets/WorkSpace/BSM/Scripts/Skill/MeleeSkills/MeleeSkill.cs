using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public abstract class MeleeSkill : SkillFactory
{
    private ParticleSystem.MainModule mainModule;
    private Vector2 currentDirection;
    
    protected float skillDamage;
    
    public MeleeSkill(SkillNode skillNode) : base(skillNode)
    {
    }

    public abstract float GetSkillDamage();

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

    protected void SlowEffect(IEffectReceiver monster, float duration)
    {
        monster.ReceiveSlow(duration);
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