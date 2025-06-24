using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(GeneralMonsterMethod))]
[RequireComponent(typeof(GeneralAnimator))]
public class GeneralMonsterAI : MonoBehaviour
{
    [SerializeField]
    public MonsterData monsterData;
    
    //테스트씬 테스트용
    //[Inject]
    [SerializeField]
    protected TestPlayer player;

    protected BehaviourTree tree;

    protected BTNode attack;
    protected BTNode idle;
    protected BTNode chase;
    protected BTNode attackCheck;
    protected BTNode chaseCheck;

    protected BTNode selector;
    protected BTNode attackSequence;
    protected BTNode chaseSequence;

    protected MonsterStateMachine stateMachine;

    protected GeneralMonsterMethod method;

    public M_State monsterState;


    private void Start()
    {
        NodeInit();
        ExternalInit();
    }

    private void Update()
    {
        tree.Tick();
    }

    public virtual void Idle()
    {
        Debug.Log("몬스터가 대기중입니다.");
    }
    public virtual void Attack()
    {
        Debug.Log("몬스터가 공격합니다");
    }

    public virtual void Move()
    {
        Debug.Log("몬스터가 이동합니다.");
    }

    protected List<BTNode> RootSelector()
    {
        List<BTNode> nodes = new List<BTNode>();
        //Die를 맨 앞에 놔야함
       
        nodes.Add(attackSequence);
        nodes.Add(chaseSequence);
        nodes.Add(idle);

        return nodes;
    }

    protected List<BTNode> ChaseSequence()
    {
        //Todo : 현재는 예시용으로 넣은 것 추후 수정 필요
        List<BTNode> nodes = new List<BTNode>();
        nodes.Add(chaseCheck);
        nodes.Add(chase);

        return nodes;
    }

    protected List<BTNode> AttackSequence()
    {
        //Todo : 현재는 예시용으로 넣은 것 추후 수정 필요
        List<BTNode> nodes = new List<BTNode>();
        nodes.Add(attackCheck);
        nodes.Add(attack);
        nodes.Add(new WaitNode(() => method.isAttacking));
        nodes.Add(idle);

        return nodes;
    }


    void NodeInit()
    {
        attack = new AttackNode(this.Attack);
        idle = new IdleNode(this.Idle);
        chase = new ChaseNode(this.Move);
        attackCheck = new IsPreparedAttackNode(gameObject.transform, player.transform, monsterData.AttackRange, monsterData.AttackCooldown);
        chaseCheck = new IsPreparedChaseNode(gameObject.transform, player.transform, monsterData.ChaseRange);


        //예시 용
        attackSequence = new Sequence(AttackSequence());
        chaseSequence = new Sequence(ChaseSequence());


        selector = new Selector(RootSelector());
        tree = new BehaviourTree(selector);
    }

    void ExternalInit()
    {
        stateMachine = new MonsterStateMachine(GetComponent<GeneralAnimator>());
        method = GetComponent<GeneralMonsterMethod>();
        method.MonsterDataInit(monsterData);
    }

}
