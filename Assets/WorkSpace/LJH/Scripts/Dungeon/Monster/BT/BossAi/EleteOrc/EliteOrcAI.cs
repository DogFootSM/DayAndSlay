using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteOrcAI : BossMonsterAI
{
    [Header("힐 조건 조정")]
    [SerializeField] private float healThresholdPercent = 20f;
    [SerializeField] private float healCooldown = 8f;

    [Header("독 쿨타임 조정")]
    [SerializeField] private float poisonCooldown = 10f;

    [Header("공격 쿨타임 조정")]
    [SerializeField] private float attackCooldown = 2f;


    private float healTimer;
    private float poisonTimer;
    private float attackTimer;
    
     protected void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        UpdateCooldowns();

        // 테스트 코드
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    model.SetMonsterHp(-10);
        //    
        //    Debug.Log($"벨루스의 체력 -10 되어 현재 체력 {model.GetMonsterHp()}");
        //}
    }

    private void UpdateCooldowns()
    {
        healTimer -= Time.deltaTime;
        poisonTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;
    }
    
    protected override bool IsAllOnCooldown()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > monsterData.ChaseRange)
        {
            return true;
        }

        if (distance > monsterData.AttackRange)
        {
            return !CanPoison() && !CanHeal();
        }

        return !CanPoison() && !CanHeal() && !CanAttack();
    }

    private bool CanHeal()
    {
        if (healTimer > 0f) return false;

        MonsterModel myModel = model;

        return true;

    }

    private bool CanPoison()
    {
        return poisonTimer <= 0f;
    }

    private bool CanAttack()
    {
        return attackTimer <= 0f;
    }

    private void ResetHealCooldown()
    {
        healTimer = healCooldown;
    }

    private void ResetPoisonCooldown()
    {
        poisonTimer = poisonCooldown;
    }

    private void ResetAttackCooldown()
    {
        attackTimer = attackCooldown;
    }

    // ---------------- actual actions ----------------

    private void PerformHeal()
    {
        method.Skill_First();
        SkillCommonStart();
        ResetHealCooldown();
    }

    private void PerformPoison()
    {
        method.Skill_Second();
        SkillCommonStart();
        ResetPoisonCooldown();
    }

    private void PerformBite()
    {
        AttackCommonStart();
        ResetAttackCooldown();
    }

    private void EndAction()
    {
        AttackCommonEnd();
    }

    // ---------------- BT patterns ----------------


    protected override List<BTNode> BuildSkillSelector()
    {
        List<BTNode> list = new List<BTNode>();

        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(CanHeal),
            new ActionNode(PerformHeal),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
            new ActionNode(EndAction)
        }));

        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(CanPoison),
            new ActionNode(PerformPoison),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
            new ActionNode(EndAction)
        }));

        return list;
    }

    protected override List<BTNode> BuildAttackSelector()
    {
        List<BTNode> list = new List<BTNode>();

        list.Add(new Sequence(new List<BTNode>
        {
            new IsAttackRangeNode(transform, player.transform, 2f),
            new IsPreparedCooldownNode(CanAttack),
            new ActionNode(PerformBite),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
            new ActionNode(EndAction)
        }));

        return list;
    }
}
