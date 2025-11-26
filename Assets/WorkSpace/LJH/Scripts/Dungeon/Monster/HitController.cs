using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour
{
    [SerializeField] private GameObject _hitEffectCanvas;
    
    private HitEffectPool _hitEffectPool => HitEffectPool.Instance;

    

    /// <summary>
    /// 데미지 텍스트 애니메이션 실행
    /// </summary>
    /// <param name="damage"></param>
    public void ActiveHitEffect(float damage)
    {
        //풀에서 텍스트 오브젝트를 꺼내옴
        GameObject instance = _hitEffectPool.GetHitEffectInPool();
        
        instance.transform.SetParent(_hitEffectCanvas.transform);
        instance.transform.localPosition = Vector3.zero;
        
        instance.GetComponent<HitEffect>().SetScaleHitEffect(damage);
        
        instance.SetActive(true);
    }
    
}
