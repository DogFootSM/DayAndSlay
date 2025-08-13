using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteOrcAI : BossMonsterAI
{
    [Header("돌진 쿨타임 조정")]
    [SerializeField] private float rushCooldown = 10f;

    [Header("공격 쿨타임 조정")]
    [SerializeField] private float attackCooldown = 2f;


    private float rushTimer;
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
        rushTimer -= Time.deltaTime;
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
            return !CanRush();
        }

        return !CanRush() && !CanAttack();
    }

    private bool CanRush()
    {
        return rushTimer <= 0f;
    }

    private bool CanAttack()
    {
        return attackTimer <= 0f;
    }

    private void ResetPoisonCooldown()
    {
        rushTimer = rushCooldown;
    }

    private void ResetAttackCooldown()
    {
        attackTimer = attackCooldown;
    }

    // ---------------- actual actions ----------------

    private void PerformRush()
    {
        method.Skill_First();
        SkillCommonStart();
        ResetPoisonCooldown();
    }

    private void PerformAttack()
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
            new IsPreparedCooldownNode(CanRush),
            new ActionNode(PerformRush),
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
            new ActionNode(PerformAttack),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
            new ActionNode(EndAction)
        }));

        return list;
    }
}
