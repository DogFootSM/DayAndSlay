using System.Collections.Generic;
using UnityEngine;

public class Malus : NepenthesAI
{
    [Header("힐 조건 조정")]
    [SerializeField] private float healThresholdPercent = 20f;
    [SerializeField] private float skillFirstCooldown = 8f;

    [Header("뿌리 공격 쿨타임 조정")]
    [SerializeField] private float rootCooldown = 5f;

    [Header("공격 쿨타임 조정")]
    [SerializeField] private float attackCooldown = 2f;

    [Header("Bellus")]
    [SerializeField] private BossMonsterAI partner;

    private float healTimer;
    private float skillFirstTimer;
    private float rootTimer;

    protected void Start()
    {
        base.Start();
        partner = FindObjectOfType<Bellus>();
    }
    protected override void Update()
    {
        base.Update();
        UpdateCooldowns();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            model.SetMonsterHp(-10);
            
            Debug.Log($"벨루스의 체력 -10 되어 현재 체력 {model.GetMonsterHp()}");
        }
    }

    private void UpdateCooldowns()
    {
        healTimer -= Time.deltaTime;
        skillFirstTimer -= Time.deltaTime;
        rootTimer -= Time.deltaTime;
    }
    
    protected override bool IsAllOnCooldown()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > monsterData.ChaseRange)
        {
            Debug.Log("거리 멀어서 트루");
            return true;
        }

        if (distance > monsterData.AttackRange)
        {
            Debug.Log("거리 중간에 스킬들 쿨이라 트루");
            return !CanRoot() && !CanSkillFirst();
        }

        Debug.Log("다 만족해서 트루");
        return !CanRoot() && !CanSkillFirst() && !CanAttack();
    }

    private bool CanSkillFirst()
    {
        if (partner == null) return false;
        if (healTimer > 0f) return false;

        MonsterModel myModel = model;
        MonsterModel partnerModel = partner.GetComponent<MonsterModel>();
        if (myModel == null || partnerModel == null) return false;

        float myHpPercent = (float)myModel.GetMonsterHp() / (float)myModel.GetMonsterMaxHp() * 100f;
        float partnerHpPercent = (float)partnerModel.GetMonsterHp() / (float)partnerModel.GetMonsterMaxHp() * 100f;
        float diff = Mathf.Abs(myHpPercent - partnerHpPercent);

        return diff >= healThresholdPercent;
    }

    private bool CanRoot()
    {
        return skillFirstTimer <= 0f;
    }

    private bool CanAttack()
    {
        return rootTimer <= 0f;
    }

    private void ResetSkillFirstCooldown()
    {
        healTimer = skillFirstCooldown;
    }

    private void ResetRootCooldown()
    {
        skillFirstTimer = rootCooldown;
    }

    private void ResetAttackCooldown()
    {
        rootTimer = attackCooldown;
    }

    // ---------------- actual actions ----------------

    private void PerformSkillFirst()
    {
        method.Skill_First();
        SkillCommonStart();
        ResetSkillFirstCooldown();
    }

    private void PerformRoot()
    {
        method.Skill_Second();
        SkillCommonStart();
        ResetRootCooldown();
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

    protected override List<BTNode> BuildSkillPatterns()
    {
        List<BTNode> list = new List<BTNode>();

        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(CanSkillFirst),
            new ActionNode(PerformSkillFirst),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
            new ActionNode(EndAction)
        }));

        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(CanRoot),
            new ActionNode(PerformRoot),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
            new ActionNode(EndAction)
        }));

        return list;
    }

    protected override List<BTNode> BuildAttackPatterns()
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

    public BossMonsterAI GetPartner() => partner;
}
