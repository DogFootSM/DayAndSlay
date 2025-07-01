using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(GeneralMonsterMethod))]
[RequireComponent(typeof(GeneralAnimator))]
[RequireComponent (typeof(MonsterModel))]
public class GeneralMonsterAI : MonoBehaviour
{
    [SerializeField]
    public MonsterData monsterData;
    
    //테스트씬 테스트용
    [SerializeField]
    protected GameObject player;

    protected BehaviourTree tree;

    protected BTNode die;
    protected BTNode attack;
    protected BTNode idle;
    protected BTNode chase;

    protected BTNode dieCheck;
    protected BTNode attackCheck;
    protected BTNode chaseCheck;

    protected BTNode selector;
    protected BTNode dieSequence;
    protected BTNode attackSequence;
    protected BTNode chaseSequence;

    protected MonsterStateMachine stateMachine;
    protected MonsterModel model;
    protected GeneralMonsterMethod method;

    public M_State monsterState;


    private void Start()
    {
        Init();
        NodeInit();
        ExternalInit();
    }

    private void Update()
    {
        tree.Tick();

        /// <summary>
        /// 테스트용 : 몬스터 죽이기 코드
        /// </summary>
        //if (Input.GetKeyDown(KeyCode.Space))
        //    model.Hp = 0;
    }

    public virtual void Hit()
    {
        //Toodo : 히트 만들어줘야함
    }
    public virtual void Die()
    {
        Debug.Log("몬스터가 사망했습니다");
        method.Die();
        stateMachine.ChangeState(new MonsterDieState());
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
        nodes.Add(dieSequence);
        nodes.Add(attackSequence);
        nodes.Add(chaseSequence);
        nodes.Add(idle);

        return nodes;
    }

    protected List<BTNode> DieSequence()
    {
        //Todo : 현재는 예시용으로 넣은 것 추후 수정 필요
        List<BTNode> nodes = new List<BTNode>();
        nodes.Add(dieCheck);
        nodes.Add(die);

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
        //nodes.Add(new WaitNode(() => !method.isAttacking));
        
        return nodes;
    }

    public IEnumerator AttackEndDelay()
    {
        //임시로 시간 지정
        float animeLength = 0.5f; 
        yield return new WaitForSeconds(animeLength);

        method.isAttacking = false;
        monsterState = M_State.IDLE;

    }


    void NodeInit()
    {
        die = new ActionNode(this.Die);
        attack = new ActionNode(this.Attack);
        idle = new ActionNode(this.Idle);
        chase = new ActionNode(this.Move);

        dieCheck = new IsDieNode(() => model.Hp);
        attackCheck = new IsPreparedAttackNode(gameObject.transform, player.transform, monsterData.AttackRange, monsterData.AttackCooldown);
        chaseCheck = new IsPreparedChaseNode(gameObject.transform, player.transform, monsterData.ChaseRange, monsterData.AttackRange);


        //예시 용
        attackSequence = new Sequence(AttackSequence());
        chaseSequence = new Sequence(ChaseSequence());
        dieSequence = new Sequence(DieSequence());

        selector = new Selector(RootSelector());
        tree = new BehaviourTree(selector);
    }

    void ExternalInit()
    {
        stateMachine = new MonsterStateMachine(GetComponent<GeneralAnimator>());
        method = GetComponent<GeneralMonsterMethod>();
        method.MonsterDataInit(monsterData);
    }

    void Init()
    {
        player = GameObject.FindWithTag("Player");
        model = GetComponent<MonsterModel>();
    }

}
