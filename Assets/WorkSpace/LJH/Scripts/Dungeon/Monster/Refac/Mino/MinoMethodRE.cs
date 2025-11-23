using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class MinoMethodRE : BossMethodRE
{
    private BossMonsterAI mino;

    
    [SerializeField] private Labyrinth labyrinth;
    [SerializeField] private ParticleSystem labyrinthEffect;

    [SerializeField] private float buffDuration;

    public override void Skill_First()
    {
        //Todo : 부딪힌 적에게 피해
        HeadButt();
    }

    public override void Skill_Second()
    {
        //Todo : 범위 내 적에게 피해
        Stomp();
    }

    public override void Skill_Third()
    {
        Gigantism();
    }

    public override void Skill_Fourth()
    {
        Debug.Log("스킬 4 실행");
        Labyrinth();
    }

    private void HeadButt()
    {
        Vector3 skillPos;

        Direction direction = GetDirectionToTarget(transform.position, player.transform.position);

        switch (direction)
        {
            case Direction.Up:
                skillPos = transform.position + new Vector3(0, 1, 0);
                break;
            
            case Direction.Down:
                skillPos = transform.position + new Vector3(0, -1, 0);
                break;
            
            case Direction.Right:
                skillPos = transform.position + new Vector3(1, 0, 0);
                break;
            
            case Direction.Left:
                skillPos = transform.position + new Vector3(-1, 0, 0);
                break;
            
            default:
                skillPos = transform.position + new Vector3(0, -1, 0);
                break;
        }
        
        
        firstSkillData.SetSkillRadius(skillPos);
    }

    private void Stomp()
    {
        secondSkillData.SetSkillRadius(transform.position);
    }
    

    private void Gigantism()
    {
        transform.localScale = new Vector3(6, 6, 6);
        monsterData.Attack *= 2;
        monsterData.AttackRange += 3;
        monsterData.MoveSpeed += 3;
    }

    private void DisGigantism()
    {
        transform.localScale = new Vector3(3, 3, 3);
        monsterData.Attack /= 2;
        monsterData.AttackRange -= 3;
        monsterData.MoveSpeed -= 3;
    }

    private void Labyrinth()
    {
        Debug.Log("미궁을 생성합니다.");
        player.transform.position = transform.position;
        labyrinthEffect.Play();

        labyrinth.gameObject.SetActive(true);
        //Todo 미궁 생성
    }

    public override void DieMethod()
    {
        Debug.Log("사망");
        
        //사망 이펙트 재생
        //DropItem();
        DisGigantism();
        LabyrinthOff();
        DungeonManager.Instance.BossDoorOpen();
        Destroy(gameObject);
    }

    public void LabyrinthOff()
    {
        labyrinth.gameObject.SetActive(false);
    }

    public void SetLabyrinth(Labyrinth labyrinth) => this.labyrinth = labyrinth;

}