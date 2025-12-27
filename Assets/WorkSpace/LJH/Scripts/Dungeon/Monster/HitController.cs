using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour
{
    [SerializeField] private GameObject _hitEffectCanvas;
    
    private HitEffectPool _hitEffectPool => HitEffectPool.Instance;

    

    /// <summary>
    /// 히트이펙트 켜주기
    /// </summary>
    /// <param name="damage"></param>
    public void ActiveHitEffect(float damage)
    {
        //풀에서 텍스트 오브젝트를 꺼내옴
        GameObject instance = _hitEffectPool.GetHitEffectInPool();
        
        //부모 설정
        instance.transform.SetParent(_hitEffectCanvas.transform);
        instance.transform.localPosition = Vector3.zero;
        
        //히트 이펙트 크기 조정
        instance.GetComponent<HitEffect>().SetScaleHitEffect(damage);
        
        instance.SetActive(true);
    }
    
}
