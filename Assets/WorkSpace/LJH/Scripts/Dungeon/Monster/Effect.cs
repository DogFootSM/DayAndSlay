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

    [SerializeField] private SpriteRenderer warningEffect;
    
    private Coroutine dotCoroutine;

    public void PlaySkill()
    {

        StartCoroutine(PlayCoroutine());
    }

    private IEnumerator PlayCoroutine()
    {
        SetWarning();
        yield return new WaitForSeconds(delay);
        GetComponent<ParticleSystem>().Play();
    }

    private void Start()
    {   
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
        /*if (_warningEffect == null) return;
        
        ParticleSystem.MainModule main = _warningEffect.main;
        main.startLifetime =  new ParticleSystem.MinMaxCurve(0.5f);
        
        _warningEffect.transform.position = transform.position;
        _warningEffect.Play();*/
        
        if (warningEffect == null) return;
        
        StartCoroutine(WarningCoroutine(delay));
        
        
    }

    private IEnumerator WarningCoroutine(float delay)
    {
        if (warningEffect != null)
        {
            warningEffect.gameObject.SetActive(true);

            yield return new WaitForSeconds(delay);

            warningEffect.gameObject.SetActive(false);
        }

        yield return null;

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