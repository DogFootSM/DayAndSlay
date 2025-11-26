using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private float maxScale = 1.5f;
    private float middleScale = 1f;
    private float minScale = 0.75f;
    
    private HitEffectPool _hitEffectPool =>  HitEffectPool.Instance;
    public void SetScaleHitEffect(float damage)
    {
        if (damage >= 50f)
        {
            transform.localScale = new Vector3(maxScale, maxScale, maxScale);
        }
        else if (damage >= 10f)
        {
            transform.localScale = new Vector3(middleScale, middleScale, middleScale);
        }
        else if (damage >= 1f)
        {
            transform.localScale = new Vector3(minScale, minScale, minScale);
        }
    }

    private void OnEnable()
    {
        transform.position = transform.parent.position + new Vector3(Random.Range(-0.75f, 1), Random.Range(-1, 1), Random.Range(-1, 1));

        StartCoroutine(DisableEffectPoolCoroutine());
    }
    
    private IEnumerator DisableEffectPoolCoroutine()
    {
        yield return new WaitForSeconds(0.25f);
        _hitEffectPool.ReturnHitEffectInPool(gameObject);
    }
}
