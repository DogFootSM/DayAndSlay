using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Malus : NepenthesAI
{
    private float skillCooldown = 10f;
    private float rangeAttackCooldown = 10f;
    private float attackCooldown = 2f;

    private float skillTimer = 0f;
    private float rangeAttackTimer = 0f;
    private float attackTimer = 0f;

    private void UpdateCooldowns()
    {
        skillTimer -= Time.deltaTime;
        rangeAttackTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;
    }

    private bool CanSkill() => skillTimer <= 0f;
    private bool CanRangeAttack() => rangeAttackTimer <= 0f;
    private bool CanAttack() => attackTimer <= 0f;

    private void ResetCooldown(Action action)
    {
        if (action == PerformSkill) skillTimer = skillCooldown;
        else if (action == PerformRangeAttack) rangeAttackTimer = rangeAttackCooldown;
        else if (action == PerformAttack) attackTimer = attackCooldown;
    }

    // =============== 실제 스킬 메서드 ===============
    private void PerformSkill()
    {
        Debug.Log("Malus: 부하 소환!");
        //animator.Play("Heal");
        ResetCooldown(PerformSkill);
        //StartCoroutine(AttackEndDelay());
    }

    private void PerformRangeAttack()
    {
        Debug.Log("Malus: 촉수 공격!");
        //animator.Play("Poison");
        ResetCooldown(PerformRangeAttack);
        //StartCoroutine(AttackEndDelay());
    }

    private void PerformAttack()
    {
        Debug.Log("Malus: 물기 공격!");
        //animator.Play("Bite");
        ResetCooldown(PerformAttack);
        //StartCoroutine(AttackEndDelay());
    }

    // =============== 비헤이비어 트리 빌드 ===============
    protected override List<BTNode> SelectorMethod()
    {
        return new List<BTNode>
        {
            // 특수 스킬(힐)
            new Sequence(new List<BTNode>
            {
                new IsAttackRangeNode(transform, player.transform, 12f),
                new IsPreparedCooldownNode(CanSkill),
                new ActionNode(PerformSkill)
            }),

            // 원거리 공격(독장판)
            new Sequence(new List<BTNode>
            {
                new IsAttackRangeNode(transform, player.transform, 7f),
                new IsPreparedCooldownNode(CanRangeAttack),
                new ActionNode(PerformRangeAttack)
            }),

            // 근거리 공격(물기)
            new Sequence(new List<BTNode>
            {
                new IsAttackRangeNode(transform, player.transform, 2f),
                new IsPreparedCooldownNode(CanAttack),
                new ActionNode(PerformAttack)
            })
        };
    }

    protected override void Update()
    {
        base.Update();
        UpdateCooldowns();  // 쿨타임 갱신
    }
}