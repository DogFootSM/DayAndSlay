using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectReceiver
{
    public void TakeDamage(float damage);
    
    /// <summary>
    /// 넉백 리시버
    /// 몬스터에 따른 넉백 효과 구현
    /// 플레이어의 위치의 공격 방향으로 넉백
    /// </summary>
    /// <param name="playerPos"></param>
    /// <param name="playerDir"></param>
    public void ReceiveKnockBack(Vector2 playerPos, Vector2 playerDir);
    
    /// <summary>
    /// 도트 데미지 리시버
    /// 몬스터에 따른 도트 데미지 구현
    /// tick에 따른 도트 데미지 피해
    /// </summary>
    /// <param name="duration">도트 데미지의 지속 시간</param>
    /// <param name="tick">몬스터에게 데미즈를 가할 시간 간격</param>
    /// <param name="damage">도트 데미지 수치</param>
    public void ReceiveDot(float duration, float tick, float damage);
    
    /// <summary>
    /// 스턴 효과 리시버
    /// 몬스터에 따른 스턴 효과 구현
    /// 지속 시간에 따른 몬스터 스턴
    /// </summary>
    /// <param name="duration">스턴 지속 시간</param>
    public void ReceiveStun(float duration);

    /// <summary>
    /// 둔화 효과 리시버
    /// 몬스터에 따른 둔화 효과 구현
    /// 지속시간에 따른 이동속도 감소
    /// </summary>
    /// <param name="duration">디버프 지속 시간</param>
    /// <param name="ratio">디버프 감소 비율</param>
    public void ReceiveSlow(float duration, float ratio);

    /// <summary>
    /// 몬스터 방어력 감소 효과 리시버
    /// 지속 시간에 따른 몬스터 방어력 감소
    /// </summary>
    /// <param name="duration">스킬 효과 지속 시간</param>
    /// <param name="deBuffPercent">방어력 감소할 비율</param>
    public void ReceiveDefenseDeBuff(float duration, float deBuffPercent);
    
    /// <summary>
    /// 타겟 표시 아이콘 On Off
    /// </summary>
    public void ReceiveMarkOnTarget();
}
