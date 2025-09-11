using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ParticleInteraction : MonoBehaviour
{
    public string EffectId;
    
    private SkillParticlePooling instance => SkillParticlePooling.Instance;
    protected List<Action> actions = new List<Action>();
    private Coroutine projectileCo;

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
    public void LinearProjectile(float delaySeconds, Vector2 targetDirection, float maxDistance)
    {
        if (projectileCo != null)
        {
            StopCoroutine(projectileCo);
            projectileCo = null;
        }
        
        transform.right = targetDirection;
         
        projectileCo = StartCoroutine(ParticleToMonsterPosRoutine(delaySeconds, maxDistance));
    }

    /// <summary>
    /// 몬스터 방향 이동 코루틴
    /// </summary>
    private IEnumerator ParticleToMonsterPosRoutine(float delaySeconds, float maxDistance)
    {
        yield return WaitCache.GetWait(delaySeconds);
        
        Vector2 startPos = transform.position;
 
        while (Vector2.Distance(transform.position, startPos) < maxDistance)
        {
            transform.Translate(Vector2.right * 15f * Time.deltaTime);

            yield return null;      
        } 
        
        //스킬 사정거리까지 날아간 후 파티클 정지
        gameObject.GetComponent<ParticleSystem>().Stop();
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
