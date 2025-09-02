using System;
using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [Header("타입")]
    [SerializeField] private DamageType skillType;
    [Header("스킬 시작 지연 시간")]
    [SerializeField] private float delay;
    [Header("지속 시간(이펙트 지속 시간)")]
    [SerializeField] private float duration;
    [Header("틱 (몇초마다 타격하는지 입력)")]
    [SerializeField] private float tick;
    [Header("값 (총 데미지를 입력)")]
    [SerializeField] private float damage;

    [SerializeField] ParticleSystem warningEffect;
    
    private Action<float, float, float> skillAction;
    
    private Coroutine dotCoroutine;

    private void Start()
    {
        SetType();
        if (skillType == DamageType.INSTANTDAMAGE)
        {
            tick = 999999999999999999999999f;
        }
        
        else if (skillType == DamageType.DOTDAMAGE)
        {
            GetComponent<ParticleSystem>().startLifetime = duration;
            float damagePerTick = damage / (duration / tick);
            
            damage = damagePerTick;
        }

        if (delay >= 1f)
        {
            WarningEffect();
        }
    }

    private void WarningEffect()
    {
        warningEffect.transform.position = transform.position;
        warningEffect.Play();
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("파티클이 " + other.name + "에 충돌했습니다!");
        Debug.Log($"{gameObject.name} {damage}damage");
        
        // 예시: IEffectReceiver 인터페이스를 가진 컴포넌트 찾기
        if (other.TryGetComponent<IEffectReceiver>(out IEffectReceiver receiver))
        {
            // 원하는 로직 실행
            receiver.TakeDamage(damage);
        }
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

    private IEnumerator DotCoroutine(Collider2D other)
    {
        while (true)
        {
            //IEffectReceiver receiver = other.GetComponent<IEffectReceiver>();
            //other.GetComponent<PlayerController>().TakeDamage(receiver, damage);

            Debug.Log($"{damage} 만큼 피해 입음");
            yield return new WaitForSeconds(tick);
        }
        
    }
    
    //인스턴트 데미지의 경우 이펙트에 닿으면 바로 데미지 처리
    //도트 데미지의 경우 이펙트에 닿은 동안 지속적으로 데미지 처리
    //인스턴트 힐의 경우 즉시 체력 회복
    //도트 힐의 경우 지속적으로 체력 회복

    private void SetType()
    {
        switch (skillType)
        {
            case DamageType.INSTANTDAMAGE:
                //skillAction = InstantDamage;
                break;
            
            case DamageType.DOTDAMAGE:
                //skillAction = DotDamage;
                break;
                                                   
        } 
    }


}