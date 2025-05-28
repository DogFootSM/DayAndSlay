using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class GeneralMonsterAI : MonoBehaviour
{
    [SerializeField]
    MonsterData monsterData;
    [Inject]
    TestPlayer player;

    BehaviourTree tree;

    BTNode attack;
    BTNode idle;
    BTNode chase;
    BTNode attackCheck;

    BTNode selector;
    BTNode attackSequence;

    private void Start()
    {
        if (player == null)
            Debug.Log("플레이어 널임");

        attack = new AttackNode(this.Attack);
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
        tree.Tick();
    }

    void Idle()
    {
        //Todo : 몬스터 대기 상태
        Debug.Log("몬스터가 대기중입니다.");
    }
    void Attack()
    {
        //Todo :몬스터 공격 구현해야함
        Debug.Log("몬스터가 공격합니다");
    }

    void Move()
    {
        //Todo : 몬스터 이동 구현해야함
        Debug.Log("몬스터가 이동합니다.");
    }

    List<BTNode> RootSelector()
    {
        List<BTNode> nodes = new List<BTNode>();
       
        nodes.Add(attackSequence);
        nodes.Add(chase);
        nodes.Add(idle);

        return nodes;
    }

    List<BTNode> AttackSequence()
    {
        //Todo : 현재는 예시용으로 넣은 것 추후 수정 필요
        List<BTNode> nodes = new List<BTNode>();
        nodes.Add(attackCheck);
        nodes.Add(attack);

        return nodes;
    }

}
