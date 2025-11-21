using System.Collections;
using UnityEngine;
using Zenject;

public abstract class BossMonsterMethod : MonsterMethod
{
    public ParticleSystem effect;
    
    
    public abstract override void Skill_First();
    public abstract override void Skill_Second();
    public abstract override void Skill_Third();
    public abstract override void Skill_Fourth();
    
    public void SetPosEffect(ParticleSystem effect, GameObject target)
    {
        effect.transform.position = target.transform.position;
    }

    public override void AttackMethod()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();

        if (playerController.IsParrying)
        {

            ai.TakeDamage(100);
            parryingCount++;
            if (parryingCount < 3)
            {
                ai.ReceiveStun(1);
            }
            animator.StartCoroutine(animator.PlayCounterCoroutine());

            if (parryingCount == 3)
            {
                parryingCount = 0;
            }

            return;
        }


        Debug.Log(ai);
        Debug.Log(ai.GetDirectionByAngle(player.transform.position, transform.position));
        
        Direction direction = ai.GetDirectionByAngle(player.transform.position, transform.position);
        
        float distance = Vector2.Distance(player.transform.position, transform.position);

        //몬스터가 보는 방향과 플레이어와의 방향 비교)
        if (animator.GetCurrentDir() == direction)
        {
            //사거리 비교
            if (distance <= ai.GetMonsterModel().AttackRange)
            {
                Debug.Log("몬스터 공격");
                PlayerHpDamaged(monsterData.Attack);
                
            }
        }

    }

    public void EffectPlay()
    {
        if (effect == null)
        {
            Debug.Log("effect가 Null입니다.");
            return;
        }
        effect.Play(true);
    }
    
    public override void DieMethod()
    {
        Debug.Log("사망");
        
        //사망 이펙트 재생
        //DropItem();
        DungeonManager.Instance.BossDoorOpen();
        Destroy(gameObject);
    }
}