using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class GeneralMonsterAI : MonoBehaviour
{
    [SerializeField]
    protected MonsterData monsterData;
    [Inject]
    protected TestPlayer player;

    protected GeneralAnimator animator;

    protected BehaviourTree tree;

    protected BTNode attack;
    protected BTNode idle;
    protected BTNode chase;
    protected BTNode attackCheck;

    protected BTNode selector;
    protected BTNode attackSequence;

    public bool isAttacking = false;



    private void Start()
    {
        animator = GetComponent<GeneralAnimator>();

        attack = new AttackNode(this.Attack, GetComponent<Animator>(), "MonsterAttackLeft", this);
        idle = new IdleNode(this.Idle);
        chase = new ChaseNode(this.Move);
        attackCheck = new IsPreparedAttackNode
            (gameObject.transform, player.transform, monsterData.range, monsterData.cooldown);

        //예시 용
        attackSequence = new Sequence(AttackSequence());



        selector = new Selector(RootSelector());
        tree = new BehaviourTree(selector);
    }



    private void Update()
    {
        Debug.Log(isAttacking);
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
       
        nodes.Add(attackSequence);
        nodes.Add(chase);
        nodes.Add(idle);

        return nodes;
    }

    protected List<BTNode> AttackSequence()
    {
        //Todo : 현재는 예시용으로 넣은 것 추후 수정 필요
        List<BTNode> nodes = new List<BTNode>();
        nodes.Add(attackCheck);
        nodes.Add(attack);

        return nodes;
    }

}
