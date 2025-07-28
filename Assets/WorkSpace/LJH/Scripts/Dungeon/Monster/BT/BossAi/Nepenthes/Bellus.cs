using System;
using System.Collections.Generic;
using UnityEngine;

public class Bellus : NepenthesAI
{
    [Header("힐 조건 조정")]
    [SerializeField] private float healThresholdPercent = 20f;
    [SerializeField] private float healCooldown = 8f;

    [Header("독 쿨타임 조절")]
    [SerializeField] private float poisonCooldown = 10f;

    [Header("공격 쿨타임 조정")]
    [SerializeField] private float attackCooldown = 2f;

    [Header("Malus")]
    [SerializeField] private BossMonsterAI partner;

    [Header("스킬용 테스트 오브젝트")] 
    [SerializeField] private GameObject heal;
    [SerializeField] private GameObject poison;

    private float healTimer;
    private float poisonTimer;
    private float attackTimer;

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
        
        heal.SetActive(true);
        Invoke("TestSetActiveHeal", 2f);
        method.Skill();
        SkillCommonStart();
        ResetHealCooldown();
    }

    private void PerformPoison()
    {
        SkillCommonStart();
        
        poison.SetActive(true);
        Invoke("TestSetActivePoison", 2f);
        
        Vector3 spawnPos = player.transform.position + (Vector3)(UnityEngine.Random.insideUnitCircle * 1.5f);

        ResetPoisonCooldown();
    }

    private void TestSetActiveHeal()
    {
        heal.SetActive(false);
    }

    private void TestSetActivePoison()
    {
        poison.SetActive(false);
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
