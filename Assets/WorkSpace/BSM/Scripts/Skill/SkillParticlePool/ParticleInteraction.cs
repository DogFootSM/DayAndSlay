using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ParticleInteraction : MonoBehaviour
{
    [SerializeField] private GameObject hitParticlePrefab;
    public string EffectId;
    
    private SkillParticlePooling instance => SkillParticlePooling.Instance;
    private Coroutine projectileCo;
    private List<Action> actions = new List<Action>();

    private bool isProjectileStopped;
    private string hitEffectId;

    public bool IsProjectileStopped
    {
        get => isProjectileStopped;
        set => isProjectileStopped = value;
    }

    private void OnEnable()
    {
        isProjectileStopped = false;
    }

    /// <summary>
    /// 실행할 스킬 효과를 받아옴
    /// </summary>
    /// <param name="actions">실행할 스킬 효과 함수들</param>
    public void ReceiveAction(List<Action> actions)
    {
        this.actions = actions;
    }

    /// <summary>
    /// 일직선 발사체 파티클 몬스터 방향으로 이동
    /// </summary>
    /// <param name="delaySeconds">딜레이 시간, 몇 초 후에 몬스터 방향으로 이동할지</param>
    /// <param name="targetDirection">파티클이 날라갈 방향</param>
    public void LinearProjectile(float delaySeconds, Vector2 targetDirection, float maxDistance, float projectileSpeed = 15f)
    {
        if (projectileCo != null)
        {
            StopCoroutine(projectileCo);
            projectileCo = null;
        }
        
        transform.right = targetDirection;
         
        projectileCo = StartCoroutine(ParticleToMonsterPosRoutine(delaySeconds, maxDistance, projectileSpeed));
    }

    /// <summary>
    /// 몬스터 방향 이동 코루틴
    /// </summary>
    private IEnumerator ParticleToMonsterPosRoutine(float delaySeconds, float maxDistance, float projectileSpeed = 15f)
    {
        yield return WaitCache.GetWait(delaySeconds);
        
        Vector2 startPos = transform.position;
        ParticleSystem particleSystem = gameObject.GetComponent<ParticleSystem>();

        while (Vector2.Distance(transform.position, startPos) < maxDistance)
        {
            transform.Translate(Vector2.right * projectileSpeed * Time.deltaTime);

            //날라가는 이펙트 중지 상태가 됐을 경우
            if (isProjectileStopped)
            {
                //기존에 날라가던 이펙트 중지
                particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                break;
            }
            
            yield return null;      
        }

        //파티클이 재생중인 상태일 때
        if (particleSystem.isPlaying)
        {
            //스킬 사정거리까지 날아간 후 파티클 정지
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    /// <summary>
    /// 타격 이펙트 재생
    /// </summary>
    public void PlayHitEffect()
    {
        //타격 이펙트 풀에서 꺼내옴
        GameObject hitInstance = SkillParticlePooling.Instance.GetSkillPool(hitEffectId, hitParticlePrefab);
        hitInstance.SetActive(true);
        hitInstance.transform.position = transform.position;
        hitInstance.transform.right = transform.right;
                
        //타격 이펙트 풀에 반납할 ID 설정
        ParticleInteraction hitInteraction = hitInstance.GetComponent<ParticleInteraction>();
        hitInteraction.EffectId = hitEffectId;
                
        //타격 이펙트 재생
        ParticleSystem hitParticleSystem = hitInstance.GetComponent<ParticleSystem>();
        hitParticleSystem.Play();
    }
    
    /// <summary>
    /// 타격 이펙트를 풀에 반납할 ID 설정
    /// </summary>
    /// <param name="hitEffectId">풀에 반납할 ID</param>
    public void SetHitEffectId(string hitEffectId)
    {
        this.hitEffectId = hitEffectId;
    }
    
    /// <summary>
    /// 파티클 정지 시 풀에 반환 이벤트
    /// </summary>
    private void OnParticleSystemStopped()
    { 
        instance.ReturnSkillParticlePool(EffectId, gameObject);
    }
    
    private void OnParticleTrigger()
    { 
        foreach (var action in actions)
        {
            action?.Invoke();
        }
    } 
}
