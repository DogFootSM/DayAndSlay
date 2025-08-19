using System.Collections.Generic;
using UnityEngine;

public class MinoAI : BossMonsterAI
{
    [Header("박치기 쿨타임 조정")]
    [SerializeField] private float buttCooldown = 10f;

    [Header("밟기 쿨타임 조정")] 
    [SerializeField] private float stompCooldown = 10f;
    
    [Header("공격 쿨타임 조정")]
    [SerializeField] private float attackCooldown = 2f;

    private float buttTimer;
    private float stompTimer;
    private float attackTimer;
    
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
        buttTimer -= Time.deltaTime;
        stompTimer -= Time.deltaTime;
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
            return !CanButt() && !CanStomp();
        }

        return !CanButt() && !CanStomp() && !CanAttack();
    }

    private bool CanButt()
    {
        return buttTimer <= 0f;
    }

    private bool CanStomp()
    {
        return stompTimer <= 0f;
    }

    private bool CanAttack()
    {
        return attackTimer <= 0f;
    }

    private void ResetButtCooldown()
    {
        buttTimer = buttCooldown;
    }

    private void ResetStompCooldown()
    {
        stompTimer = stompCooldown;
    }
    
    private void ResetAttackCooldown()
    {
        attackTimer = attackCooldown;
    }

    // ---------------- actual actions ----------------

    private void PerformButt()
    {
        method.Skill_First();
        SkillCommonStart();
        ResetButtCooldown();
    }

    private void PerformStomp()
    {
        method.Skill_Second();
        SkillCommonStart();
        ResetStompCooldown();
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
            new IsPreparedCooldownNode(CanButt),
            new ActionNode(PerformButt),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
            new ActionNode(EndAction)
        }));
        
        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(CanStomp),
            new ActionNode(PerformStomp),
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
