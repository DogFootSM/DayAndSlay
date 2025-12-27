using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class BossMethod : MonsterMethod
{
    [SerializeField] protected SkillStorage skills;
    [SerializeField] protected MonsterSkillData firstSkillData;
    [SerializeField] protected MonsterSkillData secondSkillData;
    [SerializeField] protected MonsterSkillData thirdSkillData;
    [SerializeField] protected MonsterSkillData fourthSkillData;

    public BossAI bossAi;

    public abstract override void Skill_First();
    public abstract override void Skill_Second();
    public abstract override void Skill_Third();
    public abstract override void Skill_Fourth();


    private readonly HashSet<GameObject> _hitPlayers = new HashSet<GameObject>();
    
    
    public void SetPosEffect(ParticleSystem effect, GameObject target)
    {
        effect.transform.position = target.transform.position;
    }

    private bool Parrying()
    {
        if (_player == null)
            _player = player.GetComponent<PlayerController>();

        if (!_player.IsParrying)
            return false;

        ai.TakeDamage(
            _player.GetComponent<PlayerModel>().FinalPhysicalDamage * 1.5f);

        parryingCount++;

        if (parryingCount >= 3)
        {
            StunMethod(1f);
            parryingCount = 0;
        }

        animator.StartCoroutine(animator.PlayCounterCoroutine());
        return true;
    }

    public override void AttackMethod()
    {
        if (_player == null)
            _player = player.GetComponent<PlayerController>();

        Direction direction = ai.GetDirectionByAngle(
            player.transform.position, transform.position);

        float distance = Vector2.Distance(
            player.transform.position, transform.position);

        if (animator.GetCurrentDir() != direction)
            return;

        if (distance > ai.GetMonsterModel().AttackRange)
            return;

        if (Parrying())
            return;

        PlayerHpDamaged(monsterData.Attack);
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

    private Vector2 size;
    private Vector2 center;
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
            
            BoxCollider2D attackCollider = data.AttackCollider as BoxCollider2D;
            
            size = new(attackCollider.size.x * transform.localScale.x, attackCollider.size.y * transform.localScale.y) ;
            center = attackCollider.transform.position;

            LayerMask playerLayer = LayerMask.GetMask("Player");

            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(center, size, 0, playerLayer);


            _hitPlayers.Clear();

            // 4. 피해 처리
            foreach (Collider2D hit in hitColliders)
            {
                if (_hitPlayers.Contains(hit.gameObject)) 
                    continue;

                PlayerController pc = hit.GetComponent<PlayerController>();
                
                if (pc == null) continue;

                if (Parrying())
                {
                    _hitPlayers.Add(hit.gameObject);
                    continue;
                }

                PlayerHpDamaged((int)data.Damage);
                Debug.Log($"Overlap Hit: {data.Damage}");

                _hitPlayers.Add(hit.gameObject);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, size);
    }


    public override void StunMethod(float duration)
    {
        ai.ReceiveStun(duration);
        
        StartCoroutine(WarningDeColorCoroutine(skills.GetSkillWarning(firstSkillData), 0));
        StartCoroutine(WarningDeColorCoroutine(skills.GetSkillWarning(secondSkillData), 0));
        StartCoroutine(WarningDeColorCoroutine(skills.GetSkillWarning(thirdSkillData), 0));
        StartCoroutine(WarningDeColorCoroutine(skills.GetSkillWarning(fourthSkillData), 0));
    }
    public override void DieMethod()
    {
        Debug.Log("사망");
        
        base.DieMethod();
        DungeonManager.Instance.RemainingBossCount--;
    }
    
    
}