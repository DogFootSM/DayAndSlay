using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chomb : MonoBehaviour
{
    [SerializeField] private Animator chombAnimator;

    private Coroutine explosionDelayCo;
    private LayerMask monsterLayerMask;
    private float damage;
    private float delayTime;
    private float stunDuration = 2f;
    private int explosionAnimHash;
    private string effectId;
    
    private void Awake()
    {
        explosionAnimHash = Animator.StringToHash("Explosion");
        monsterLayerMask = LayerMask.GetMask("Monster");
    }

    private void OnEnable()
    {
        delayTime = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //몬스터 레이어 감지
        if ((1 << other.gameObject.layer & monsterLayerMask) != 0)
        {
            //폭발 트리거 On
            chombAnimator.SetTrigger(explosionAnimHash);
            
            IEffectReceiver receiver = other.gameObject.GetComponent<IEffectReceiver>();
            
            if (explosionDelayCo == null)
            {
                explosionDelayCo = StartCoroutine(ExplosionDelayRoutine(receiver));
            }
        }
    }
 
    private void OnDisable()
    {
        if (explosionDelayCo != null)
        {
            StopCoroutine(explosionDelayCo);
            explosionDelayCo = null;
        }
    }
 
    /// <summary>
    /// 폭발 딜레이 코루틴
    /// </summary>
    /// <param name="receiver">처음으로 감지된 몬스터 객체</param>
    /// <returns></returns>
    private IEnumerator ExplosionDelayRoutine(IEffectReceiver receiver)
    {
        while (true)
        {
            //현재 재생중인 애니메이션이 폭발 애니메이션인지?
            if (chombAnimator.GetCurrentAnimatorStateInfo(0).IsName("Chomb_Bomb_Clip"))
            {
                delayTime = chombAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                
                //애니메이션이 종료됐으면 데미지 실행
                if (delayTime >= 1f)
                {
                    //Hit
                    receiver.TakeDamage(damage);
                    receiver.ReceiveStun(stunDuration);
                    
                    SkillParticlePooling.Instance.ReturnSkillParticlePool(effectId, gameObject);
                    break;
                }
            }

            yield return null;
        } 
    }

    /// <summary>
    /// 스킬 데이터 설정
    /// </summary>
    /// <param name="damage"></param>
    public void SetSkillData(float damage, string effectId)
    {
        this.damage = damage;
        this.effectId = effectId;
    }
}