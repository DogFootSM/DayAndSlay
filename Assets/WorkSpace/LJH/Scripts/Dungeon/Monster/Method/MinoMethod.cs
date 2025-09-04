using System;
using System.Collections;
using UnityEngine;

public class MinoMethod : BossMonsterMethod
{
    private BossMonsterAI mino;

    [SerializeField] private ParticleSystem stompEffect;
    
    [SerializeField] private Labyrinth labyrinth;
    [SerializeField] private ParticleSystem labyrinthEffect;

    [SerializeField] private float buffDuration;

    public override void Skill_First()
    {
        //Todo : 부딪힌 적에게 피해
    }

    public override void Skill_Second()
    {
        //Todo : 범위 내 적에게 피해
        stompEffect.Play();
    }

    public override void Skill_Third()
    {
        Gigantism();
    }

    public override void Skill_Fourth()
    {
        Debug.Log("스킬 4 실행");
        Labirinth();
    }

    private void Gigantism()
    {
        transform.localScale = new Vector3(6, 6, 6);
        monsterData.Attack += monsterData.Attack;
        monsterData.AttackRange += 3;
        monsterData.MoveSpeed += 3;
    }

    private void Labirinth()
    {
        Debug.Log("미궁을 생성합니다.");
        player.transform.position = transform.position;
        labyrinthEffect.Play();
        labyrinth.gameObject.SetActive(true);
        //Todo 미궁 생성
    }

}
    
