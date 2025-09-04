using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ParticleInteraction : MonoBehaviour
{
    [FormerlySerializedAs("SkillID")] public string EffectId;
    
    private SkillParticlePooling instance => SkillParticlePooling.Instance;
    private List<Action> actions = new List<Action>();
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
    /// 파티클 몬스터 방향으로 이동
    /// </summary>
    /// <param name="delaySeconds">딜레이 시간, 몇 초 후에 몬스터 방향으로 이동할지</param>
    /// <param name="targetPos">파티클이 날라갈 위치</param>
    public void ParticleToMonsterPos(float delaySeconds, Vector2 targetPos)
    {
        if (projectileCo != null)
        {
            StopCoroutine(projectileCo);
            projectileCo = null;
        }

        projectileCo = StartCoroutine(ParticleToMonsterPosRoutine(delaySeconds, targetPos));
    }

    /// <summary>
    /// 몬스터 방향 이동 코루틴
    /// </summary>
    private IEnumerator ParticleToMonsterPosRoutine(float delaySeconds, Vector2 targetPos)
    {
        yield return WaitCache.GetWait(delaySeconds);

        while (Vector2.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime * 5f);
            yield return null;
        } 
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
