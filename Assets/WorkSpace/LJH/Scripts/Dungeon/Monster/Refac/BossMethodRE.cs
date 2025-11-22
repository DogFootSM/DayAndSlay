using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class BossMethodRE : MonsterMethod
{
    private PlayerController _player;
        
    [SerializeField] protected MonsterSkillData firstSkillData;
    [SerializeField] protected MonsterSkillData secondSkillData;
    [SerializeField] protected MonsterSkillData thirdSkillData;
    [SerializeField] protected MonsterSkillData fourthSkillData;
    
    public abstract override void Skill_First();
    public abstract override void Skill_Second();
    public abstract override void Skill_Third();
    public abstract override void Skill_Fourth();

    
    private readonly HashSet<GameObject> _hitPlayers = new HashSet<GameObject>();
    
    
    
    public void SetPosEffect(ParticleSystem effect, GameObject target)
    {
        effect.transform.position = target.transform.position;
    }

    public override void AttackMethod()
    {
        if(_player == null)
            _player = player.GetComponent<PlayerController>();
        
        if (_player.IsParrying)
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

        
        Direction direction = ai.GetDirectionByAngle(player.transform.position, transform.position);
        
        float distance = Vector2.Distance(player.transform.position, transform.position);

        //몬스터가 보는 방향과 플레이어와의 방향 비교)
        if (animator.GetCurrentDir() == direction)
        {
            //사거리 비교
            if (distance <= ai.GetMonsterModel().AttackRange)
            {
                PlayerHpDamaged(monsterData.Attack);
                
            }
        }
    }

    public void WarningPlay(MonsterSkillData skill)
    {
        SpriteRenderer warningImage = SkillStorage.instance.GetSkillWarning(skill);
        
        if (warningImage == null)
        {
            Debug.Log("effect가 Null입니다.");
            return;
        }

        warningImage.gameObject.SetActive(true);
    }

    public void EffectPlay(MonsterSkillData skill)
    {
        Effect effect = SkillStorage.instance.GetSkillVFX(skill);

        if (SkillStorage.instance.GetSkillWarning(skill) != null)
        {
            SpriteRenderer warningImage = SkillStorage.instance.GetSkillWarning(skill);
            
            warningImage.gameObject.SetActive(false);
        }


        if (effect == null)
        {
            Debug.Log("effect가 Null입니다.");
            return;
        }

        effect.PlaySkill();
    }
    
    public void Animation_DoHitCheck(int skillIndex) 
    {
        // 1. 사용할 스킬 데이터에 맞는 콜라이더 정보를 가져옵니다.
        MonsterSkillData data = null;
    
        if (skillIndex == 1) 
        {
            data = firstSkillData;
        }
        else if (skillIndex == 2)
        {
            data = secondSkillData;
        }

        data.AttackCollider = SkillStorage.instance.GetSkillRadius(data);
        
        if (data == null || data.AttackCollider == null) return;
    
        Collider2D attackCollider = data.AttackCollider;
    
        // 2. 공격 영역 내에 있는 모든 Collider2D를 찾습니다.
        // OverlapCircleAll(원의 중심 위치, 원의 반지름, 검색 레이어)
        // 여기서는 AttackCollider의 위치와 크기 정보를 사용하여 검색합니다.
    
        // Collider의 크기와 형태에 따라 OverlapCircleAll, OverlapBoxAll 등을 선택합니다.
    
        // 예시: OverlapCircleAll 사용
        float radius = attackCollider.bounds.extents.x; // 콜라이더의 크기를 반지름으로 대략 사용
        Vector2 center = attackCollider.transform.position;
    
        // 플레이어 레이어만 검색하도록 LayerMask 설정 (예: LayerMask.GetMask("Player"))
        LayerMask playerLayer = LayerMask.GetMask("Player"); 
    
        // 겹쳐진 모든 콜라이더를 배열로 가져옵니다.
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius, playerLayer);

        // 3. 중복 피해 방지를 위해 HashSet을 매 공격마다 초기화하고 사용합니다.
        // BossMethodRE 클래스 레벨에 private readonly HashSet<GameObject> _hitPlayers = new HashSet<GameObject>(); 추가 필요
        _hitPlayers.Clear(); 
    
        // 4. 피해 처리
        foreach (Collider2D hit in hitColliders)
        {
            // 이미 피해를 입혔다면 건너뛰기 (한 번의 공격당 한 번만 피해)
            if (_hitPlayers.Contains(hit.gameObject)) continue;

            if (hit.GetComponent<PlayerController>())
            {
                PlayerHpDamaged((int)data.Damage);
                Debug.Log($"Overlap Hit: {data.Damage}");
            }
            // 플레이어에게 피해를 주는 함수 호출 (예: hit.GetComponent<PlayerController>().TakeDamage((int)data.Damage);)
        
            _hitPlayers.Add(hit.gameObject);
        }
    
        // Note: 이 방식은 AttackEnd 함수가 따로 필요 없습니다. 공격 판정이 이 함수가 실행되는 한 프레임에 종료됩니다.
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