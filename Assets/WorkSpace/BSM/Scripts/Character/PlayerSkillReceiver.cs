using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerSkillReceiver : MonoBehaviour
{
    [SerializeField] private PlayerModel playerModel;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Image tempCastingEffect;
    [SerializeField] private GameObject tempShieldEffect;

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

    private bool isPowerTradeBuffActive;
    private bool isDefenceTradeBuffActive;
    private LayerMask monsterMask;

    private Queue<IEffectReceiver> monsterQueue = new Queue<IEffectReceiver>();

    private int playerLayer;
    private int monsterLayer;

    private bool isDashDone;
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
            playerModel.FinalPhysicalDefense = (int)(playerModel.PlayerStats.PhysicalDefense * defenseIncrease);
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
    /// <param name="castingTime">스킬 캐스팅 시간</param>
    /// <param name="shieldCount">충전할 보호막 횟수</param>
    /// <param name="defenseBoostMultiplier">증가할 방어력 값</param>
    /// <param name="duration">스킬의 지속 시간</param>
    public void ReceiveShield(float castingTime, int shieldCount, float defenseBoostMultiplier, float duration)
    {
        if (castingCo != null)
        {
            StopCoroutine(castingCo);
            castingCo = null;
        }

        if (shieldSkillCo != null)
        {
            StopCoroutine(shieldSkillCo);
            shieldSkillCo = null;
        }

        castingCo = StartCoroutine(SkillCastingRoutine(castingTime));
        shieldSkillCo = StartCoroutine(ShieldRoutine(shieldCount, defenseBoostMultiplier, duration));
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
    /// 스킬 캐스팅 시간 코루틴
    /// </summary>
    /// <param name="castingTime">스킬 사용에 필요한 준비 시간</param>
    /// <returns></returns>
    private IEnumerator SkillCastingRoutine(float castingTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime < castingTime)
        {
            elapsedTime += Time.deltaTime;
            tempCastingEffect.fillAmount = elapsedTime / castingTime;
            yield return null;
        }

        tempCastingEffect.fillAmount = 0;
        playerModel.IsCastingDone = true;
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

        //캐스팅이 끝날때까지 대기
        yield return new WaitUntil(() => playerModel.IsCastingDone);

        //캐스팅 상태 초기화
        playerModel.IsCastingDone = false;

        //쉴드 개수 및 추가 쉴드량 변경
        playerModel.ShieldCount = shieldCount;
        playerModel.DefenseBoostMultiplier = defenseBoostMultiplier;
        //TODO: 모델의 CastingSpeed는 뭐어떻게?
        //TODO: 쉴드 이펙트로 변경 필요 및 캐스팅 이펙트는 MeleeEffect로 사용하는게 맞으려나?
        tempShieldEffect.SetActive(true);

        while (playerModel.ShieldCount > 0 && elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tempShieldEffect.SetActive(false);

        //쉴드 개수 및 추가 쉴드량 초기화
        playerModel.DefenseBoostMultiplier = 0;
        playerModel.ShieldCount = 0;
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

    public void ReceiveMoveSpeedBuff(float duration, float ratio)
    {
        if (moveSpeedBuffCo != null)
        {
            StopCoroutine(moveSpeedBuffCo);
            moveSpeedBuffCo = null;
        }

        moveSpeedBuffCo = StartCoroutine(MoveSpeedBuffRoutine(duration, ratio));
    }

    /// <summary>
    /// 이동 속도 증가 버프
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator MoveSpeedBuffRoutine(float duration, float ratio)
    {
        float originSpeed = playerModel.PlayerStats.baseMoveSpeed;
        playerModel.PlayerStats.baseMoveSpeed += (playerModel.PlayerStats.baseMoveSpeed * ratio);

        yield return WaitCache.GetWait(duration);
        playerModel.PlayerStats.baseMoveSpeed = originSpeed;
    }
}