using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toadstool : MonoBehaviour
{
    [SerializeField] private Animator toadstoolAnimController;
    [SerializeField] private SpriteRenderer toadstoolRenderer;

    private Coroutine targetLocationCo;
    private GameObject effectPrefab;

    private LayerMask monsterLayer;
    
    private string searchAnimationName = "Toadstool_Explosion_Wait";
    private string effectId;
    private string explosionEffectId;
    private int explosionWaitHash;
    private int upMoveHash;
    private int downMoveHash;
    private int sideMoveHash;
    private float skillDamage;
    private float deBuffDuration;
    private float tick = 1f;
    
    private void Awake()
    {
        InitAnimHash();
        InitLayerMask();
    }

    /// <summary>
    /// 애니메이션 해시 초기화
    /// </summary>
    private void InitAnimHash()
    {
        explosionWaitHash = Animator.StringToHash("Toadstool_Explosion_Wait");
        upMoveHash = Animator.StringToHash("Toadstool_Up_Move");
        downMoveHash = Animator.StringToHash("Toadstool_Down_Move");
        sideMoveHash = Animator.StringToHash("Toadstool_Side_Move");
    }

    /// <summary>
    /// 레이어 초기화
    /// </summary>
    private void InitLayerMask()
    {
        monsterLayer = LayerMask.GetMask("Monster");
    }
    
    private void OnDisable()
    {
        if (targetLocationCo != null)
        {
            StopCoroutine(targetLocationCo);
            targetLocationCo = null;
        }
    }

    /// <summary>
    /// 스킬 데이터 설정
    /// </summary>
    /// <param name="effectId">스킬 풀에 반환할 이펙트 아이디</param>
    /// <param name="skillDamage">스킬 데미지</param>
    /// <param name="effectPrefab">버섯이 터진 후 재생할 이펙트 프리팹</param>
    public void SetSkillData(string effectId, float skillDamage, GameObject effectPrefab, string prefabId, float duration)
    {
        this.effectId = effectId;
        this.skillDamage = skillDamage;
        this.effectPrefab = effectPrefab;

        explosionEffectId = prefabId;
        deBuffDuration = duration;
    }
    
    /// <summary>
    /// 이동할 위치 설정 및 이동
    /// </summary>
    /// <param name="direction">이동할 방향</param>
    /// <param name="distance">이동할 최대 사거리</param>
    public void SetTargetLocation(Vector2 direction, float distance)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            toadstoolRenderer.flipX = direction.x < 0;
            toadstoolAnimController.Play(sideMoveHash);
        }
        else
        {
            toadstoolRenderer.flipX = false;
            
            if (direction.y > 0)
            {
                toadstoolAnimController.Play(upMoveHash);
            }
            else
            {
                toadstoolAnimController.Play(downMoveHash);
            }
        }

        if (targetLocationCo == null)
        {
            targetLocationCo = StartCoroutine(GoToTargetLocationRoutine(direction, distance));
        }
    }

    private IEnumerator GoToTargetLocationRoutine(Vector2 dir, float distance)
    {
        Vector2 startPos = transform.position;

        while (Vector2.Distance(transform.position, startPos) < distance)
        {
            transform.Translate(dir * 2f * Time.deltaTime);
            yield return null;
        }
        
        toadstoolAnimController.Play(explosionWaitHash);

        while (true)
        {
            if (toadstoolAnimController.GetCurrentAnimatorStateInfo(0).IsName(searchAnimationName))
            {
                float currentStateTime = toadstoolAnimController.GetCurrentAnimatorStateInfo(0).normalizedTime;
                
                if (currentStateTime >= 1)
                {
                    ExplosionEffectPlay();
                    break;
                } 
            } 
            yield return null;
        }
    }

    /// <summary>
    /// 폭발 애니메이션 재생
    /// </summary>
    private void ExplosionEffectPlay()
    {
        GameObject explosionInstance = SkillParticlePooling.Instance.GetSkillPool(explosionEffectId, effectPrefab);
        explosionInstance.SetActive(true);
        explosionInstance.transform.position = transform.position;
        
        ParticleSystem particleSystem = explosionInstance.GetComponent<ParticleSystem>();
        particleSystem.Play();

        ParticleInteraction particleInteraction = explosionInstance.GetComponent<ParticleInteraction>();
        particleInteraction.EffectId = explosionEffectId;

        ExecuteExplosion();
    }

    /// <summary>
    /// 폭발 데미지 실행
    /// </summary>
    private void ExecuteExplosion()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 1.2f, monsterLayer);
        //감지된 몬스터 수만큼 타격
        for (int i = 0; i < cols.Length; i++)
        {
            IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
            receiver.TakeDamage(skillDamage);
            receiver.ReceiveDot(deBuffDuration, tick, skillDamage * 0.2f);
        }
        
        SkillParticlePooling.Instance.ReturnSkillParticlePool(effectId,gameObject);
    } 
}
