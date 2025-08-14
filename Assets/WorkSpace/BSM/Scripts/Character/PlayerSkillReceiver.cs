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

    public void ReceiveExecuteCharge(Vector2 chargeDirection, Vector2 playerPos, Vector2 overlapSize)
    {
        playerController.CharacterRb.velocity = chargeDirection * 5f;

        if (dashCo != null)
        {
            StopCoroutine(dashCo);
            dashCo = null;
        }

        if (dashStunCo != null)
        {
            StopCoroutine(dashStunCo);
            dashStunCo = null;
        }

        dashCo = StartCoroutine(DashRoutine(chargeDirection, playerPos, overlapSize));
        dashStunCo = StartCoroutine(DashStunRoutine(chargeDirection, playerPos, overlapSize));
    }

    private IEnumerator DashRoutine(Vector2 chargeDirection, Vector2 playerPos, Vector2 overlapSize)
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

    private IEnumerator DashStunRoutine(Vector2 dir, Vector2 playerPos, Vector2 overlapSize)
    {
        yield return new WaitUntil(() => isDashDone);
        
        Collider2D[] overlaps =
            Physics2D.OverlapBoxAll(playerPos + (dir.normalized * 1f), overlapSize, 0, monsterMask);

        foreach (var col in overlaps)
        {
            col.GetComponent<IEffectReceiver>().ReceiveStun(3f);
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
        float originSpeed = playerModel.PlayerStats.moveSpeed;
        playerModel.PlayerStats.moveSpeed += (playerModel.PlayerStats.moveSpeed * ratio);

        yield return WaitCache.GetWait(duration);
        playerModel.PlayerStats.moveSpeed = originSpeed;
    }
    
}