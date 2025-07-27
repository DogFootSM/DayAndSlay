using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bellus : NepenthesAI
{
    [SerializeField] private GameObject heal;
    [SerializeField] private GameObject poison;
    private float healCooldown = 10f;
    private float poisonCooldown = 10f;
    private float biteCooldown = 2f;

    private float healTimer = 0f;
    private float poisonTimer = 0f;
    private float biteTimer = 0f;

    private void UpdateCooldowns()
    {
        healTimer -= Time.deltaTime;
        poisonTimer -= Time.deltaTime;
        biteTimer -= Time.deltaTime;
    }

    private bool CanHeal() => healTimer <= 0f;
    private bool CanPoison() => poisonTimer <= 0f;
    private bool CanBite() => biteTimer <= 0f;

    private void ResetCooldown(Action action)
    {
        if (action == PerformHeal) healTimer = healCooldown;
        else if (action == PerformPoison) poisonTimer = poisonCooldown;
        else if (action == PerformBite) biteTimer = biteCooldown;
    }

    // =============== 실제 스킬 메서드 ===============
    private void PerformHeal()
    {
        Debug.Log("Bellus: 힐 사용!");
        //animator.Play("Heal");
        ResetCooldown(PerformHeal);
        //StartCoroutine(AttackEndDelay());
        heal.SetActive(true);
    }

    private void PerformPoison()
    {
        Debug.Log("Bellus: 독장판 설치!");
        //animator.Play("Poison");
        ResetCooldown(PerformPoison);
        //StartCoroutine(AttackEndDelay());
        poison.SetActive(true);
    }

    private void PerformBite()
    {
        Debug.Log("Bellus: 물기 공격!");
        //animator.Play("Bite");
        ResetCooldown(PerformBite);
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
                new IsPreparedCooldownNode(CanHeal),
                new ActionNode(PerformHeal)
            }),

            // 원거리 공격(독장판)
            new Sequence(new List<BTNode>
            {
                new IsAttackRangeNode(transform, player.transform, 7f),
                new IsPreparedCooldownNode(CanPoison),
                new ActionNode(PerformPoison)
            }),

            // 근거리 공격(물기)
            new Sequence(new List<BTNode>
            {
                new IsAttackRangeNode(transform, player.transform, 2f),
                new IsPreparedCooldownNode(CanBite),
                new ActionNode(PerformBite)
            })
        };
    }

    protected override void Update()
    {
        base.Update();
        UpdateCooldowns();  // 쿨타임 갱신
    }
}