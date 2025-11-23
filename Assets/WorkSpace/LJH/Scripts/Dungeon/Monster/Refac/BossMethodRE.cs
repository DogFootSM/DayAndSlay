using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class BossMethodRE : MonsterMethod
{
    [SerializeField] protected SkillStorage skills;
    [SerializeField] protected MonsterSkillData firstSkillData;
    [SerializeField] protected MonsterSkillData secondSkillData;
    [SerializeField] protected MonsterSkillData thirdSkillData;
    [SerializeField] protected MonsterSkillData fourthSkillData;

    public BossAIRe bossAi;
    
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


        // SpriteRenderer 리스트
        List<SpriteRenderer> warningImages = new List<SpriteRenderer>();

        // 자식들 순회
        for (int i = 0; i < skills.GetSkillWarning(skill).Count; i++)
        {
            SpriteRenderer sr = skills.GetSkillWarning(skill)[i];
            if (sr != null)
                warningImages.Add(sr);
        }

        // 실제로 스프라이트가 없는 경우 체크
        if (warningImages.Count == 0)
        {
            Debug.Log("Warning SpriteRenderer가 없음");
            return;
        }

        // 전부 활성화
        StartCoroutine(WarningColorCoroutine(warningImages, 0.4f));
    }

    private IEnumerator WarningColorCoroutine(List<SpriteRenderer> list, float delay)
    {
        foreach (var sprite in list)
        {
            Color tempCol = sprite.color;
            tempCol.a = 0.3411f;
            sprite.color = tempCol;
            
            yield return new WaitForSeconds(delay);
        }
    }

    public void EffectPlay(MonsterSkillData skill)
    {
        // VFX 객체 가져오기

            List<SpriteRenderer> warnings = new List<SpriteRenderer>();

            // warningRoot 자식들 중 SpriteRenderer만 OFF
            for (int i = 0; i < skills.GetSkillWarning(skill).Count; i++)
            {
                SpriteRenderer sr = skills.GetSkillWarning(skill)[i].GetComponent<SpriteRenderer>();
                if (sr != null)
                    warnings.Add(sr);
            }

            // 비활성화
            StartCoroutine(WarningDeColorCoroutine(warnings, 0.4f));

        // 스킬 VFX 실행
        StartCoroutine(EffectPlayCoroutine(skills.GetSkillVFX(skill), 0.4f));
    }
    
    private IEnumerator WarningDeColorCoroutine(List<SpriteRenderer> list, float delay)
    {
        foreach (var sprite in list)
        {
            Color tempCol = sprite.color;
            tempCol.a = 0;
            sprite.color = tempCol;
            
            yield return new WaitForSeconds(delay);
        }
    }
    
    private IEnumerator EffectPlayCoroutine(List<Effect> list, float delay)
    {

        if (list.Count == 0)
        {
            Debug.LogWarning("list의 갯수가 0입니다!");
        }
        foreach (var effect in list)
        {
            effect.PlaySkill();
            yield return new WaitForSeconds(delay);
        }
    }
    
    public void Animation_DoHitCheck(int skillIndex) 
    {
        MonsterSkillData data = null;
    
        if (skillIndex == 1) 
        {
            data = firstSkillData;
        }
        else if (skillIndex == 2)
        {
            data = secondSkillData;
        }
        else if (skillIndex == 3)
        {
            data = thirdSkillData;
        }
        
        else if (skillIndex == 4)
        {
            data = fourthSkillData;
        }

        foreach (var tempCollider in skills.GetSkillRadius(data))
        {
            data.AttackCollider =  tempCollider;

            if (data == null || data.AttackCollider == null) return;

            Collider2D attackCollider = data.AttackCollider;

            float radius = attackCollider.bounds.extents.x;
            Vector2 center = attackCollider.transform.position;

            LayerMask playerLayer = LayerMask.GetMask("Player");

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius, playerLayer);

            _hitPlayers.Clear();

            // 4. 피해 처리
            foreach (Collider2D hit in hitColliders)
            {
                if (_hitPlayers.Contains(hit.gameObject)) continue;

                if (hit.GetComponent<PlayerController>())
                {
                    PlayerHpDamaged((int)data.Damage);
                    Debug.Log($"Overlap Hit: {data.Damage}");
                }

                _hitPlayers.Add(hit.gameObject);
            }
        }
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