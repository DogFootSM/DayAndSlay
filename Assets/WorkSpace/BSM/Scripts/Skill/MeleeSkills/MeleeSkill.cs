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
    protected List<List<Action>> skillActions = new List<List<Action>>();
    protected List<ParticleSystem.MainModule> mainModules = new List<ParticleSystem.MainModule>();
    protected List<ParticleSystem.TriggerModule> triggerModules = new List<ParticleSystem.TriggerModule>();
    protected List<ParticleInteraction> interactions = new List<ParticleInteraction>();
    protected ParticleSystemRenderer particleSystemRenderer;
    protected ParticleSystem particleSystem;
    
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
        skillActions.Clear();
        mainModules.Clear();
        triggerModules.Clear();
        interactions.Clear();
    }
    
    protected void SetOverlapSize(Vector2 direction, float skillRange)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            //TODO: 감지 모양 및 크기 추후 수정
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
            return skillNode.skillData.SkillDamage * skillNode.CurSkillLevel * (1f + skillNode.PlayerModel.NextSkillDamageMultiplier);
        } 
        
        return skillNode.skillData.SkillDamage * skillNode.CurSkillLevel;
    }

    /// <summary>
    /// 레벨당 디버프 지속 시간 증가 계산
    /// </summary>
    /// <returns></returns>
    protected float GetDeBuffDurationIncreasePerLevel(float increaseValue)
    {
        return skillNode.skillData.DeBuffDuration + ((skillNode.CurSkillLevel -1) * increaseValue);
    }
    
    /// <summary>
    /// 근접 공격 이펙트
    /// </summary>
    protected void SkillEffect(Vector2 position, int index, string effectId, GameObject skillEffectPrefab)
    { 
        instance = particlePooling.GetSkillPool(effectId, skillEffectPrefab);
        instance.transform.parent = null;
        instance.transform.position = position;
        
        particleSystem = instance.GetComponent<ParticleSystem>();
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
    /// <param name="shieldCount">스킬 사용 시 충전할 쉴드 개수</param>
    /// <param name="defenseBoostMultiplier">쉴드 사용 시 증가할 방어력</param>
    /// <param name="duration">스킬 지속 시간</param>
    protected void ExecuteShield(int shieldCount, float defenseBoostMultiplier, float duration)
    {
        skillNode.PlayerSkillReceiver.ReceiveShield(shieldCount, defenseBoostMultiplier, duration);
    }

    /// <summary>
    /// 캐스팅 로직 호출
    /// </summary>
    /// <param name="castingTime">스킬 사용에 걸리는 시간</param>
    protected void ExecuteCasting(float castingTime)
    {
        float finalCastingTime = castingTime - skillNode.PlayerModel.CastingTimeReduction;
        
        // 캐스팅 시간이 0보다 작아지지 않도록 최소값 설정
        finalCastingTime = Mathf.Max(0, finalCastingTime);
        
        skillNode.PlayerSkillReceiver.ReceiveCasting(finalCastingTime);
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
    /// <param name="duration">버프 지속 시간</param>
    /// <param name="factor">이동 속도 증가값</param>
    /// </summary>
    protected void ExecuteMoveSpeedBuff(float duration, float factor)
    {
        skillNode.PlayerSkillReceiver.ReceiveMoveSpeedBuff(duration, factor);
    }

    /// <summary>
    /// 대상 위치 순간이동 기능 호출
    /// </summary>
    /// <param name="target">이동할 타겟 위치</param>
    protected void ExecuteBlinkToMarkedTarget(Collider2D target, Action action = null)
    {
        skillNode.PlayerSkillReceiver.ReceiveBlinkToMarkedTarget(target, action);
    }
    
    /// <summary>
    /// 몬스터 방어력 감소 디버프 효과 호출
    /// </summary>
    /// <param name="monster">감지한 몬스터</param>
    /// <param name="duration">디버프 지속 시간</param>
    /// <param name="factor">방어력 감소 비율</param>
    protected void ExecuteDefenseDeBuff(IEffectReceiver monster, float duration, float factor)
    {
        monster.ReceiveDefenseDeBuff(duration, factor);
    }

    /// <summary>
    /// 다음 스킬 데미지 버프 적용
    /// </summary>
    /// <param name="duration">데미지 버프 지속 시간</param>
    /// <param name="multiplier">데미지 증가량</param>
    protected void ExecuteNextSkillDamageBuff(float duration, float multiplier)
    {
        skillNode.PlayerSkillReceiver.ReceiveNextSkillDamageMultiplier(duration, multiplier);
    }

    /// <summary>
    /// 캐릭터 대쉬 사용
    /// </summary>
    /// <param name="dir"></param>
    protected void ExecuteDash(Vector2 dir)
    {
        skillNode.PlayerSkillReceiver.ReceiveDash(dir);
    }

    /// <summary>
    /// 파티클 트리거 리스트 감지된 콜라이더 제거
    /// </summary>
    protected void RemoveTriggerModuleList(int triggerIndex)
    {  
        //TODO:매개 변수 필요없을 경우 제거
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
    /// 레벨 당 퍼센트 기반 체력 즉시 회복
    /// </summary>
    /// <param name="healthPercent">회복할 체력의 퍼센트</param>
    protected void ExecuteRestoreHealthByPercent(float healthPercent)
    {
        skillNode.PlayerSkillReceiver.ReceiveRestoreHealthByPercent(healthPercent);
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

    /// <summary>
    /// 점프 동작 호출
    /// </summary>
    /// <param name="effectAction">점프 이후 땅에 착지했을 때 실행할 행동 리스트</param>
    protected void ExecuteJumpAttackInPlace(List<Action> effectAction = null)
    {
        skillNode.PlayerSkillReceiver.ReceiveJumpAttackInPlace(effectAction);   
    }

    /// <summary>
    /// 주변 몬스터 지속 감지 및 스킬 행동 실행
    /// </summary>
    /// <param name="radius">몬스터 감지 범위</param>
    /// <param name="tick">몇 초 간격으로 감지한 몬스터에게 행동을 취할지에 대한 시간</param>
    /// <param name="action">감지한 몬스터에게 어떤 행동을 취할지</param>
    /// <param name="duration">스킬 지속 시간</param>
    /// <typeparam name="T1"></typeparam>
    protected void ExecuteFindNearByMonsters<T1>(float radius, float tick, T1 action, float duration)
    {
        skillNode.PlayerSkillReceiver.ReceiveFindNearByMonsters(radius, tick, action, duration);
    }

    /// <summary>
    /// 스킬 이펙트 캐릭터 위치 따라 이동
    /// </summary>
    /// <param name="effectPrefab">캐릭터를 따라 다닐 스킬 이펙트</param>
    /// <param name="duration">캐릭터를 따라 다닐 지속 시간</param>
    /// <param name="effectId">캐릭터를 따라 다닐 이펙트 id</param>
    protected void ExecuteFollowCharacterWithParticle(GameObject effectPrefab, float duration, string effectId)
    {
        skillNode.PlayerSkillReceiver.ReceiveFollowCharacterWithParticle(effectPrefab, duration, effectId);
    }
    
    /// <summary>
    /// 파티클 랜덤 위치 생성
    /// </summary>
    /// <param name="spawnPos">파티클 재생 위치</param>
    /// <param name="radiusRange">스킬 범위</param>
    /// <param name="delay">스킬 이펙트 재생 지연 시간</param>
    /// <param name="particlePrefab">재생할 파티클 프리팹</param>
    /// <param name="effectId">풀에 반납할 이펙트 id</param>
    /// <param name="prefabCount">재생할 파티클 개수</param>
    protected void SpawnParticleAtRandomPosition(Vector2 spawnPos, float radiusRange, float delay, GameObject particlePrefab, string effectId, int prefabCount)
    {
        skillNode.PlayerSkillReceiver.ReceiveSpawnParticleAtRandomPosition(spawnPos, radiusRange, delay, particlePrefab, effectId, prefabCount);
    }
    
    /// <summary>
    /// 몬스터 공격력 감소 디버프 실행
    /// </summary>
    /// <param name="duration">공격력 감소 지속 시간</param>
    /// <param name="deBuffPer">공격력 감소 퍼센트</param>
    protected void ExecuteAttackDeBuffByMonster(IEffectReceiver receiver, float duration, float deBuffPer)
    {
        receiver.ReceiveAttackDeBuff(duration, deBuffPer);
    }

    /// <summary>
    /// 캐스팅 이후 수행할 동작 코루틴
    /// </summary>
    /// <param name="action">캐스팅 완료 후 수행할 동작</param>
    /// <returns></returns>
    protected IEnumerator WaitCastingRoutine(Action action)
    {
        yield return new WaitUntil(() => !skillNode.PlayerModel.IsCasting);
        action?.Invoke();
    }

    /// <summary>
    /// 캐스팅 시간 삭제 버프 실행
    /// </summary>
    /// <param name="duration">스킬 지속 시간</param>
    protected void ExecuteRemoveCast(float duration)
    {
        skillNode.PlayerSkillReceiver.ReceiveRemoveCastTime(duration);
    }

    //몬스터 위치를 저장할 임시 배열
    private Collider2D[] sortArr;
    
    /// <summary>
    /// 플레이어아 가까운 위치 기준으로 몬스터 배열 정렬
    /// </summary>
    /// <param name="cols"></param>
    /// <param name="playerPosition"></param>
    protected void SortMonstersByNearest(Collider2D[] cols, Vector2 playerPosition)
    {
        sortArr = new Collider2D[cols.Length];
        MergeSort(cols, 0, cols.Length -1, playerPosition);
    }

    /// <summary>
    /// 병합 정렬 재귀 호출
    /// </summary>
    /// <param name="arr">몬스터 원본 배열</param>
    /// <param name="start">비교 시작할 좌측 인덱스</param>
    /// <param name="end">비교 끝낼 우측 인덱스</param>
    /// <param name="playerPosition">플레이어의 위치</param>
    private void MergeSort(Collider2D[] arr, int start, int end, Vector2 playerPosition)
    {
        if (start >= end) return;

        int middle = (start + end) / 2;
        MergeSort(arr, start, middle, playerPosition);
        MergeSort(arr, middle + 1, end, playerPosition);
        Merge(arr, start, middle, end, playerPosition);
    }
    
    /// <summary>
    /// 가까운 순으로 정렬 진행
    /// </summary>
    /// <param name="arr">몬스터 원본 배열</param>
    /// <param name="start">비교 시작할 좌측 인덱스</param>
    /// <param name="middle">분할할 중앙 인덱스</param>
    /// <param name="end">비교 끝낼 우측 인덱스</param>
    /// <param name="playerPosition">플레이어 위치</param>
    private void Merge(Collider2D[] arr, int start, int middle, int end, Vector2 playerPosition)
    {
        int i = start;
        int j = middle + 1;
        int temp = 0;

        while (i <= middle && j <= end)
        {
            //플레이어 위치와 비교해 가장 가까운 순으로 정렬
            if (Vector2.Distance(playerPosition, arr[i].transform.position) <
                Vector2.Distance(playerPosition, arr[j].transform.position))
            {
                sortArr[temp++] = arr[i++];
            }
            else
            {
                sortArr[temp++] = arr[j++];
            }
        }

        while (i <= middle)
        {
            sortArr[temp++] = arr[i++];
        }

        while (j <= end)
        {
            sortArr[temp++] = arr[j++];
        }

        i = start;
        temp = 0;

        while (i <= end)
        {
            arr[i++] = sortArr[temp++];
        }
        
    }
    
}