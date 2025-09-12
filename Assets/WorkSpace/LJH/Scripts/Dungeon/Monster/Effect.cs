using System;
using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [Header("타입")]
    [SerializeField] private DamageType skillType;
    [Header("지연 타입(즉발인지 아닌지)")]
    [SerializeField] private DelayType delayType;
    [Header("스킬 시작 지연 시간")]
    [SerializeField] private float delay;
    [Header("지속 시간(이펙트 지속 시간)")]
    [SerializeField] private float duration;
    [Header("틱 (몇초마다 타격하는지 입력)")]
    [SerializeField] private float tick;
    [Header("값 (총 데미지를 입력)")]
    [SerializeField] private float damage;

    [SerializeField] ParticleSystem warningEffect;
    
    private Coroutine dotCoroutine;

    private void Start()
    {
        SetWarning();
        
        if (skillType == DamageType.INSTANTDAMAGE)
        {
            tick = 999999999999999999999999f;
        }
        
        else if (skillType == DamageType.DOTDAMAGE)
        {
            ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;

            main.startLifetime = new ParticleSystem.MinMaxCurve(duration);
            
            damage /= duration / tick;
        }
    }
    
    /// <summary>
    /// 범위 미리 표시해주는 경고용 파티클 위치 세팅 및 실행
    /// </summary>
    private void WarningEffect()
    {
        ParticleSystem.MainModule main = warningEffect.main;
        main.startLifetime =  new ParticleSystem.MinMaxCurve(0.5f);
        
        warningEffect.transform.position = transform.position;
        warningEffect.Play();
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("파티클이 " + other.name + "에 충돌했습니다!");
        Debug.Log($"{gameObject.name} {damage}damage");
        
        // 예시: IEffectReceiver 인터페이스를 가진 컴포넌트 찾기
        dotCoroutine = StartCoroutine(DotCoroutine(other));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if (dotCoroutine != null)
            {
                StopCoroutine(dotCoroutine);
            }
        }
    }

    private void OnDisable()
    {
        if (dotCoroutine != null)
        {
            StopCoroutine(dotCoroutine);
        }
    }

    private IEnumerator DotCoroutine(GameObject other)
    {
        while (true)
        {
            if (other.TryGetComponent<IEffectReceiver>(out IEffectReceiver receiver))
            {
                // 원하는 로직 실행
                receiver.TakeDamage(damage);
            }

            Debug.Log($"{damage} 만큼 피해 입음");
            yield return new WaitForSeconds(tick);
        }
        
    }

    private void SetWarning()
    {
        switch (delayType)
        {
            case DelayType.DELAY:
                WarningEffect();
                break;
            
            case DelayType.INSTANT:
                break;
        }
    }


}