using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerSkillReceiver : MonoBehaviour
{
    [SerializeField] private PlayerModel playerModel;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Image tempCastingEffect;

    public UnityAction<IEffectReceiver> MonsterCounterEvent;

    private Coroutine castingCo;
    private Coroutine shieldSkillCo;
    private Coroutine movementBlockCo;
    private Coroutine counterWhileImmobileCo;
    private Coroutine dashCo;
    private Coroutine dashStunCo;
    private Coroutine moveSpeedBuffCo;
    private Coroutine attackUpDefenseDownCo;
    private Coroutine defenseUpSpeedDownCo;
    private Coroutine BlinkToMarkCo;
    private Coroutine damageReductionCo;
    private Coroutine healthRegenCo;
    private Coroutine nextSkillDamageMultiplierCo;
    private Coroutine jumpAttackCo;
    private Coroutine skillEffectFollowCharacterCo;
    private Coroutine findNearByMonstersCo;
    private Coroutine spawnParticleAtRandomPosition;
    private Coroutine removeCastCo;
    private ParticleSystem followParticle = null;
    
    private bool isPowerTradeBuffActive;
    private bool isDefenceTradeBuffActive;
    private LayerMask monsterMask;

    private Queue<IEffectReceiver> monsterQueue = new Queue<IEffectReceiver>();

    private int playerLayer;
    private int monsterLayer;
    private bool isDashDone;
    
    private bool isNeedCasting = true;
    public bool IsDashDone => isDashDone;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        monsterLayer = LayerMask.NameToLayer("Monster");
        monsterMask = LayerMask.GetMask("Monster");
    }

    private void OnEnable()
    {
        MonsterCounterEvent += MonsterCounterRegister;
    }

    private void OnDisable()
    {
        MonsterCounterEvent -= MonsterCounterRegister;
    }

    /// <summary>
    /// 공격력 버프 & 방어력 감소 스킬 효과 리시버
    /// </summary>
    /// <param name="duration">효과 지속 시간</param>
    /// <param name="defenseDecrease">방어력 감소 값 EX)0.5 방어력 절반 감소</param>
    /// <param name="attackIncrease">공격력 증가 값 EX)1.5 현재 공격력의 1.5배로 변경</param>
    public void ReceiveAttackUpDefenseDown(float duration, float defenseDecrease, float attackIncrease)
    {
        if (isDefenceTradeBuffActive && defenseUpSpeedDownCo != null)
        {
            StopCoroutine(defenseUpSpeedDownCo);
            defenseUpSpeedDownCo = null;

            //스피드, 방어력 원상 복구
            playerModel.MoveSpeed = playerModel.GetFactoredMoveSpeed();
            playerModel.FinalPhysicalDefense = playerModel.PlayerStats.PhysicalDefense;
        }

        if (attackUpDefenseDownCo != null)
        {
            StopCoroutine(attackUpDefenseDownCo);
            attackUpDefenseDownCo = null;
        }

        attackUpDefenseDownCo = StartCoroutine(AttackUpDefenseDownRoutine(duration, defenseDecrease, attackIncrease));
    }

    private IEnumerator AttackUpDefenseDownRoutine(float duration, float defenseDecrease, float attackIncrease)
    {
        float elapsedTime = 0f;
        isPowerTradeBuffActive = true;

        while (elapsedTime < duration)
        {
            playerModel.FinalPhysicalDamage = playerModel.PlayerStats.PhysicalAttack * attackIncrease;
            playerModel.FinalPhysicalDefense = playerModel.PlayerStats.PhysicalDefense * defenseDecrease;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isPowerTradeBuffActive = false;
        playerModel.FinalPhysicalDefense = playerModel.PlayerStats.PhysicalDefense;
        playerModel.FinalPhysicalDamage = playerModel.PlayerStats.PhysicalAttack;
    }

    /// <summary>
    /// 방어력 증가 & 이동속도 감소 스킬 리시버
    /// </summary>
    /// <param name="duration">효과 지속 시간</param>
    /// <param name="speedDecrease">이동속도 감소 값</param>
    /// <param name="defenseIncrease">방어력 증가 값 EX)2 현재 방어력의 2배 수치로 변경</param>
    public void ReceiveDefenseUpSpeedDown(float duration, float speedDecrease, float defenseIncrease)
    {
        if (isPowerTradeBuffActive && attackUpDefenseDownCo != null)
        {
            StopCoroutine(attackUpDefenseDownCo);
            attackUpDefenseDownCo = null;
            //방어력, 공격력 원상 복구 
            playerModel.FinalPhysicalDefense = playerModel.PlayerStats.PhysicalDefense;
            playerModel.FinalPhysicalDamage = playerModel.PlayerStats.PhysicalAttack;
        }

        if (defenseUpSpeedDownCo != null)
        {
            StopCoroutine(defenseUpSpeedDownCo);
            defenseUpSpeedDownCo = null;
        }

        defenseUpSpeedDownCo = StartCoroutine(DefenseUpSpeedDownRoutine(duration, speedDecrease, defenseIncrease));
    }

    private IEnumerator DefenseUpSpeedDownRoutine(float duration, float speedDecrease, float defenseIncrease)
    {
        float elapsedTime = 0f;
        isDefenceTradeBuffActive = true;

        while (elapsedTime < duration)
        {
            playerModel.MoveSpeed = playerModel.GetFactoredMoveSpeed() * speedDecrease;
            playerModel.FinalPhysicalDefense = playerModel.PlayerStats.PhysicalDefense * defenseIncrease;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isDefenceTradeBuffActive = false;
        playerModel.FinalPhysicalDefense = playerModel.PlayerStats.PhysicalDefense;
        playerModel.MoveSpeed = playerModel.GetFactoredMoveSpeed();
    }

    /// <summary>
    /// 보호막 쉴드 스킬 리시버
    /// </summary>
    /// <param name="shieldCount">충전할 보호막 횟수</param>
    /// <param name="defenseBoostMultiplier">증가할 방어력 값</param>
    /// <param name="duration">스킬의 지속 시간</param>
    public void ReceiveShield(int shieldCount, float defenseBoostMultiplier, float duration)
    { 
        if (shieldSkillCo != null)
        {
            StopCoroutine(shieldSkillCo);
            shieldSkillCo = null;
        }
        
        shieldSkillCo = StartCoroutine(ShieldRoutine(shieldCount, defenseBoostMultiplier, duration));
    }

    /// <summary>
    /// 보호막 스킬 적용 코루틴
    /// </summary>
    /// <param name="shieldCount"></param>
    /// <param name="defenseBoostMultiplier"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator ShieldRoutine(int shieldCount, float defenseBoostMultiplier, float duration)
    {
        float elapsedTime = 0f;

        //쉴드 개수 및 추가 쉴드량 변경
        playerModel.ShieldCount = shieldCount;
        playerModel.DefenseBoostMultiplier = defenseBoostMultiplier;
        //TODO: 모델의 CastingSpeed는 뭐어떻게?

        while (playerModel.ShieldCount > 0 && elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        //캐릭터를 따라 이동할 이펙트 코루틴 종료 및 파티클 중지
        if (skillEffectFollowCharacterCo != null)
        {
            followParticle.Stop();
            StopCoroutine(skillEffectFollowCharacterCo);
            skillEffectFollowCharacterCo = null;
        }
        
        //쉴드 개수 및 추가 쉴드량 초기화
        playerModel.DefenseBoostMultiplier = 0;
        playerModel.ShieldCount = 0;
    }
    
    /// <summary>
    /// 마법 스킬 캐스팅 실행
    /// </summary>
    /// <param name="castingTime">캐스팅 필요 시간</param>
    public void ReceiveCasting(float castingTime)
    {
        if (castingCo == null)
        {
            castingCo = StartCoroutine(SkillCastingRoutine(castingTime));
        }
    }

    /// <summary>
    /// 스킬 캐스팅 시간 코루틴
    /// </summary>
    /// <param name="castingTime">스킬 사용에 필요한 준비 시간</param>
    /// <returns></returns>
    private IEnumerator SkillCastingRoutine(float castingTime)
    {
        float elapsedTime = 0f;
        //TODO: 캐스팅 타임은 모델에서 가져오는 방식?
        playerModel.IsCasting = true;
        
        castingTime = isNeedCasting ? castingTime : 0f;
        
        while (elapsedTime < castingTime)
        {
            elapsedTime += Time.deltaTime;
            tempCastingEffect.fillAmount = elapsedTime / castingTime;
            yield return null;
        }
        
        //캐스팅 상태 초기화
        playerModel.IsCasting = false;
        tempCastingEffect.fillAmount = 0; 
        
        if (castingCo != null)
        {
            StopCoroutine(castingCo);
            castingCo = null;
        }
    }

    /// <summary>
    /// 캐스팅 시간 삭제 버프 리시브
    /// </summary>
    /// <param name="duration">스킬 지속 시간</param>
    public void ReceiveRemoveCastTime(float duration)
    {
        if (removeCastCo == null)
        {
            removeCastCo = StartCoroutine(RemoveCastingTimeCoroutine(duration));
        } 
    }
    
    private IEnumerator RemoveCastingTimeCoroutine(float duration)
    {
        isNeedCasting = false;
        
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        isNeedCasting = true;

        if (removeCastCo != null)
        {
            StopCoroutine(removeCastCo);
            removeCastCo = null;
        } 
    }
     
    /// <summary>
    /// 대쉬 스킬 리시버
    /// </summary>
    /// <param name="chargeDirection">대쉬할 방향</param>
    public void ReceiveDash(Vector2 chargeDirection)
    {
        playerController.CharacterRb.velocity = chargeDirection * 8f;

        if (dashCo != null)
        {
            StopCoroutine(dashCo);
            dashCo = null;
        }

        dashCo = StartCoroutine(DashRoutine());
    }

    /// <summary>
    /// 대쉬 후 몬스터 스턴 동작
    /// </summary>
    /// <param name="receivers">대쉬 스킬에 맞은 몬스터</param>
    /// <param name="duration">스턴 지속 시간</param>
    public void ReceiveDashStun(IEffectReceiver receivers, float duration)
    {
        if (dashStunCo != null)
        {
            StopCoroutine(dashStunCo);
            dashStunCo = null;
        }

        dashStunCo = StartCoroutine(DashStunRoutine(receivers, duration));
    }

    /// <summary>
    /// 대쉬 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator DashRoutine()
    {
        Physics2D.IgnoreLayerCollision(playerLayer, monsterLayer, true);
        //TODO: 돌진 애니메이션 On
        isDashDone = false;
        yield return WaitCache.GetWait(0.25f);

        //돌진 애니메이션 Off
        Physics2D.IgnoreLayerCollision(playerLayer, monsterLayer, false);
        isDashDone = true;
        playerController.CharacterRb.velocity = Vector2.zero;
    }

    /// <summary>
    /// 대쉬 후 몬스터 스턴 동작 코루틴
    /// </summary>
    /// <param name="receivers">대쉬 스킬에 맞은 몬스터</param>
    /// <param name="duration">스턴 지속 시간</param>
    /// <returns></returns>
    private IEnumerator DashStunRoutine(IEffectReceiver receivers, float duration)
    {
        yield return new WaitUntil(() => isDashDone);
        receivers.ReceiveStun(duration);
    }
 
    /// <summary>
    /// 이동 불가 효과 리시버
    /// </summary>
    /// <param name="duration"></param>
    public void ReceiveMovementBlock(float duration)
    {
        if (movementBlockCo != null)
        {
            StopCoroutine(movementBlockCo);
            movementBlockCo = null;
        }

        movementBlockCo = StartCoroutine(MovementBlockRoutine(duration));
    }

    /// <summary>
    /// 지속 시간동안 캐릭터의 이동 불가 효과를 적용할 코루틴
    /// </summary>
    /// <param name="duraiton">효과 지속 시간</param>
    /// <returns></returns>
    private IEnumerator MovementBlockRoutine(float duraiton)
    {
        float elapsedTime = 0f;

        playerModel.IsMovementBlocked = true;

        while (elapsedTime < duraiton)
        {
            Debug.Log("이동 불가 상태중");
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerModel.IsMovementBlocked = false;
    }

    /// <summary>
    /// 반격 리시버
    /// </summary>
    public void ReceiveCounterWhileImmobile()
    {
        if (counterWhileImmobileCo != null)
        {
            StopCoroutine(counterWhileImmobileCo);
            counterWhileImmobileCo = null;
        }

        playerModel.IsCountering = true;
        counterWhileImmobileCo = StartCoroutine(CounterWhileImmobileRoutine());
    }

    private IEnumerator CounterWhileImmobileRoutine()
    {
        while (playerModel.IsMovementBlocked)
        {
            if (monsterQueue.Count > 0)
            {
                Debug.Log("반격 진행");
                monsterQueue.Dequeue().ReceiveStun(5f);
            }

            yield return null;
        }

        playerModel.IsCountering = false;
    }

    private void MonsterCounterRegister(IEffectReceiver monster)
    {
        monsterQueue.Enqueue(monster);
    }

    /// <summary>
    /// 이동 속도 버프 리시버
    /// </summary>
    /// <param name="duration">버프 지속 시간</param>
    /// <param name="factor">증가할 이동속도 값</param>
    public void ReceiveMoveSpeedBuff(float duration, float factor)
    {
        if (moveSpeedBuffCo != null)
        {
            StopCoroutine(moveSpeedBuffCo);
            moveSpeedBuffCo = null;
        }

        moveSpeedBuffCo = StartCoroutine(MoveSpeedBuffRoutine(duration, factor));
    }

    /// <summary>
    /// 이동 속도 증가 버프
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator MoveSpeedBuffRoutine(float duration, float factor)
    {
        playerModel.UpdateMoveSpeedFactor(factor);
        
        yield return WaitCache.GetWait(duration);
        
        playerModel.UpdateMoveSpeedFactor(0);
    }

    /// <summary>
    /// 타겟 방향 순간이동 리시버
    /// </summary>
    /// <param name="target"></param>
    public void ReceiveBlinkToMarkedTarget(Collider2D target, Action action = null)
    {
        if (BlinkToMarkCo != null)
        {
            StopCoroutine(BlinkToMarkCo);
            BlinkToMarkCo = null;
        }

        BlinkToMarkCo = StartCoroutine(BlinkToMarkedTargetRoutine(target, action));
    }

    private IEnumerator BlinkToMarkedTargetRoutine(Collider2D target, Action action = null)
    {
        while (Vector2.Distance(transform.position, target.transform.position) > 1f)
        {
            transform.position = Vector2.Lerp(transform.position, target.transform.position, 0.5f);
            yield return null;
        } 
        
        action?.Invoke();
    }

    /// <summary>
    /// 데미지 감소 비율 리시버
    /// </summary>
    /// <param name="duration">스킬 지속 시간</param>
    /// <param name="damageReduction">데미지 받을 시 얼마의 비율로 데미지 피해를 받을지에 대한 수치</param>
    public void ReceiveDamageReduction(float duration, float damageReduction)
    {
        if (damageReductionCo != null)
        {
            StopCoroutine(damageReductionCo);
            damageReductionCo = null;
        }

        damageReductionCo = StartCoroutine(DamageReductionRoutine(duration, damageReduction));
    }

    /// <summary>
    /// 데미지 감소 비율 적용 코루틴
    /// </summary>
    private IEnumerator DamageReductionRoutine(float duration, float damageReduction)
    {
        float elapsedTime = 0;

        playerModel.DamageReductionRatio = damageReduction;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerModel.DamageReductionRatio = 0;
    }

    /// <summary>
    /// 초당 체력 회복 리시버
    /// </summary>
    /// <param name="duration">체력 회복 지속 시간</param>
    /// <param name="healthRegen">초당 회복할 체력 수치</param>
    public void ReceiveHealthRegenTick(float duration, float healthRegen)
    {
        if (healthRegenCo != null)
        {
            StopCoroutine(healthRegenCo);
            healthRegenCo = null;
        }

        healthRegenCo = StartCoroutine(HealthRegenTickRoutine(duration, healthRegen));
    }

    private IEnumerator HealthRegenTickRoutine(float duration, float healthRegen)
    {
        float elapsedTime = 0;

        //지속 시간이 끝나기 전 or 현재 체력이 최대 체력보다 적을 경우 체력 회복
        while (elapsedTime < duration && playerModel.CurHp < playerModel.MaxHp)
        {
            //1초당 체력 회복
            yield return WaitCache.GetWait(1f);
            
            //전체 체력의 회복량 계산
            float regenHealth = playerModel.MaxHp * healthRegen; 
            
            //초당 회복량 현재 체력에 더함
            playerModel.CurHp += regenHealth;
            elapsedTime += 1f;
        }
    }

    /// <summary>
    /// 체력 즉시 회복 리시버
    /// </summary>
    /// <param name="healthPercent">회복할 체력의 퍼센트</param>
    public void ReceiveRestoreHealthByPercent(float healthPercent)
    {
        float healthLevelPer = playerModel.MaxHp * healthPercent;
        
        //현재 체력 + 회복할 체력 수치가 최대 체력을 넘는지 확인 후 체력 회복
        playerModel.CurHp = (playerModel.CurHp + healthLevelPer) > playerModel.MaxHp ? playerModel.MaxHp : playerModel.CurHp + healthLevelPer;
    } 
    
    /// <summary>
    /// 다음 스킬 데미지 증가 버프 적용
    /// </summary>
    /// <param name="duration">버프 적용 시간</param>
    /// <param name="multiplier">데미지 증가 수치, 레벨 당 + 0.1%</param>
    public void ReceiveNextSkillDamageMultiplier(float duration, float multiplier)
    {
        if (nextSkillDamageMultiplierCo != null)
        {
            StopCoroutine(nextSkillDamageMultiplierCo);
            nextSkillDamageMultiplierCo = null;
        }

        nextSkillDamageMultiplierCo = StartCoroutine(NextSkillDamageRoutine(duration, multiplier));
    }

    /// <summary>
    /// 다음 스킬 데미지 증가 버프 코루틴
    /// </summary>
    private IEnumerator NextSkillDamageRoutine(float duration, float multiplier)
    {
        playerModel.NextSkillDamageMultiplier = multiplier;
        playerModel.NextSkillBuffActive = true;

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //지속 시간 내 스킬 미사용 시 원상복구
        playerModel.NextSkillDamageMultiplier = 0f;
        playerModel.NextSkillBuffActive = false;
    }

    /// <summary>
    /// 제자리 점프 실행
    /// </summary>
    public void ReceiveJumpAttackInPlace(List<Action> effectAction)
    {
        if (jumpAttackCo == null)
        {
            jumpAttackCo = StartCoroutine(JumpAttackInPlaceRoutine(effectAction));
        }
    }

    /// <summary>
    /// 제자리 점프 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator JumpAttackInPlaceRoutine(List<Action> effectAction)
    {
        float originPosY = transform.position.y;
        float curHeight = transform.position.y;

        float jumpVelocity = 5f;
        float fallVelocity = 0;
        float gravity = 9f;
        
        //TODO: 점프했을 때 충돌끄고, 착지했을 때 충돌 켜야 될듯
        //제자리 점프
        while (jumpVelocity > 0)
        {
            curHeight += jumpVelocity * Time.deltaTime;
            jumpVelocity -= gravity * Time.deltaTime;
            transform.position = new Vector2(transform.position.x, curHeight);
            yield return null;
        }

        //하강
        while (curHeight > originPosY)
        {
            fallVelocity += gravity * Time.deltaTime * 5f;
            curHeight -= fallVelocity * Time.deltaTime;
            transform.position = new Vector2(transform.position.x, curHeight);
            yield return null;
        }

        //하강 완료 후 원래 y 위치로 보정
        transform.position = new Vector2(transform.position.x, originPosY);

        //땅에 착지했을 때 스킬 이펙트 Action 실행
        foreach (var effect in effectAction)
        {
            effect?.Invoke();
        }

        if (jumpAttackCo != null)
        {
            StopCoroutine(jumpAttackCo);
            jumpAttackCo = null;
        }
    }

    /// <summary>
    /// 주변 몬스터 감지 후 감지된 몬스터에게 행동을 취함
    /// </summary>
    /// <param name="radius">몬스터 감지 범위</param>
    /// <param name="tick">몇 초 간격으로 몬스터에게 행동을 취할지에 대한 시간</param>
    /// <param name="skillAction">각 스킬 별 클래스를 넘겨 받아 각 스킬의 Action 메서드를 실행하여 감지한 몬스터들에게 행동을 수행</param>
    /// <param name="duration">스킬 효과 지속 시간</param>
    /// <typeparam name="T1"></typeparam>
    public void ReceiveFindNearByMonsters<T1>(float radius, float tick, T1 skillAction, float duration)
    {
        if (findNearByMonstersCo == null)
        {
            findNearByMonstersCo = StartCoroutine(FindNearByMonstersRoutine(radius, duration, tick, skillAction));
        } 
    }

    private IEnumerator FindNearByMonstersRoutine<T1>(float radius, float duration, float tick, T1 skillAction)
    {
        Vector2 overlapSize = new Vector2(radius, radius);

        float elapsedTime = 0;
        
        //현재 주변을 감지하고 있는 상태
        while (elapsedTime < duration)
        {
            Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, overlapSize, 0, monsterMask);
            
            //사용 스킬이 SPAS009이면 아래 실행
            if (skillAction is SPAS009 spas009)
            {
                spas009.Action(cols);
            }

            yield return WaitCache.GetWait(tick);
            elapsedTime += tick;
        }
        
        //코루틴 종료 처리
        if (findNearByMonstersCo != null)
        {
            StopCoroutine(findNearByMonstersCo);
            findNearByMonstersCo = null;
        } 
    }
    
    /// <summary>
    /// 스킬 지속시간 만큼 스킬 이펙트가 캐릭터를 따라 이동
    /// </summary>
    /// <param name="effectPrefab">캐릭터를 따라 다닐 스킬 이펙트</param>
    /// <param name="duration">캐릭터를 따라 다닐 스킬 이펙트 지속 시간</param>
    /// <param name="effectId">캐릭터를 따라 다닐 스킬 이펙트 ID</param>
    public void ReceiveFollowCharacterWithParticle(GameObject effectPrefab, float duration, string effectId)
    {
        //캐릭터를 따라다닐 스킬 이펙트를 풀에서 꺼내오거나 새로 생성
        if (effectPrefab != null)
        {
            GameObject effectInstance = SkillParticlePooling.Instance.GetSkillPool(effectId, effectPrefab);
            effectInstance.SetActive(true);
            effectInstance.transform.position = transform.position + new Vector3(0, 0.2f);
        
            //캐릭터 자식으로 설정
            effectInstance.transform.parent = transform;
        
            //풀에 반납하기 위한 이펙트 ID 설정
            ParticleInteraction particleInteraction = effectInstance.GetComponent<ParticleInteraction>();
            particleInteraction.EffectId = effectId;
        
            followParticle = effectInstance.GetComponent<ParticleSystem>();
        }
        
        if (skillEffectFollowCharacterCo == null)
        {
            skillEffectFollowCharacterCo = StartCoroutine(FollowCharacterWithParticleRoutine(followParticle, duration));
        }
    }
     
    private IEnumerator FollowCharacterWithParticleRoutine(ParticleSystem particle, float duration)
    {
        float elapsedTime = 0;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        if (particle != null)
        {
            particle.Stop();
        }
         
        //코루틴 종료 처리
        if (skillEffectFollowCharacterCo != null)
        {
            StopCoroutine(skillEffectFollowCharacterCo);
            skillEffectFollowCharacterCo = null;
        }
    }

    /// <summary>
    /// 일정 범위 안에 스킬 이펙트 생성
    /// </summary> 
    public void ReceiveSpawnParticleAtRandomPosition(Vector2 spawnPos, float radiusRange, float delay,
        GameObject particlePrefab, string effectId, int prefabCount, Action lightingAction = null)
    {
        if (spawnParticleAtRandomPosition == null)
        {
            spawnParticleAtRandomPosition = StartCoroutine(SpawnParticleAtRandomPositionRoutine(spawnPos, radiusRange, delay, particlePrefab, effectId, prefabCount));
        }
    }

    private IEnumerator SpawnParticleAtRandomPositionRoutine(Vector2 spawnPos, float radiusRange, float delay,
        GameObject particlePrefab, string effectId, int prefabCount)
    {
        yield return WaitCache.GetWait(delay);
        
        //TODO: 조금만 생성하고 while 돌면서 돌려쓸지 말지 생각
        for (int i = 0; i < prefabCount; i++)
        {
            GameObject instance = SkillParticlePooling.Instance.GetSkillPool(effectId, particlePrefab);
            instance.SetActive(true);
            
            //이펙트가 생성될 랜덤 위치
            float rx = Random.Range(-radiusRange, radiusRange);
            float ry = Random.Range(-radiusRange, radiusRange);
            instance.transform.position = spawnPos + new Vector2(rx /2 , ry / 2);
            
            //풀에 반환될 이펙트 id 설정
            ParticleInteraction interaction = instance.GetComponent<ParticleInteraction>();
            interaction.EffectId = effectId;
            
            //파티클 딜레이 랜덤 설정
            ParticleSystem particleSystem = instance.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule mainModule = particleSystem.main;
            mainModule.startDelay = Random.Range(0, 5) * 0.2f;
                
            particleSystem.Play();
        }

        if (spawnParticleAtRandomPosition != null)
        {
            StopCoroutine(spawnParticleAtRandomPosition);
            spawnParticleAtRandomPosition = null;
        }
    }

    
}