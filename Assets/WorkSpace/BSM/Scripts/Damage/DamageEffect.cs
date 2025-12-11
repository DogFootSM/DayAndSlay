using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _bodyRenderer;
    [SerializeField] private Canvas _damageTextCanvas;
    
    private Coroutine _skinEffectCo;
    private DamageTextPool _damageTextPool => DamageTextPool.Instance;

    private int currentLayer;
    private DamageTextType unitType;
    
    private void Awake()
    {
        currentLayer = gameObject.layer;

        if (currentLayer == LayerMask.NameToLayer("Player"))
        {
            unitType = DamageTextType.PLAYER;
        }
        else if (currentLayer == LayerMask.NameToLayer("Monster"))
        {
            unitType = DamageTextType.MONSTER;
        } 
    }

    private void OnDisable()
    {
        if (_bodyRenderer != null)
        {
            _bodyRenderer.color = Color.white;
        }
    }

    /// <summary>
    /// 데미지 텍스트 애니메이션 실행
    /// </summary>
    /// <param name="damage"></param>
    public void DamageTextEvent(float damage)
    {
        //풀에서 텍스트 오브젝트를 꺼내옴
        GameObject instance = _damageTextPool.GetDamageTextInPool();
        
        //내 World Canvas 하위에 배치
        instance.transform.SetParent(_damageTextCanvas.transform);
        instance.transform.localPosition = Vector3.zero;
        
        //피해 입은 데미지 설정
        int ceilling = Mathf.CeilToInt(damage);
        
        DamageText damageText = instance.GetComponent<DamageText>();
        damageText.SetDamageText(ceilling.ToString(), unitType);
        
        instance.SetActive(true);
    }
    
    /// <summary>
    /// 피격 시 이펙트 재생
    /// </summary>
    public void DamageSkinEffect()
    {
        if (_skinEffectCo == null)
        {
            _skinEffectCo = StartCoroutine(DamageSkinEffectRoutine());
        }
    }

    /// <summary>
    /// 피부 Color 변경 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageSkinEffectRoutine()
    {
        float elapsedTime = 1f;
        
        //피부 컬러 Red 계열로 진행
        while (elapsedTime > 0.35f)
        {
            elapsedTime -= Time.deltaTime * 2f;

            _bodyRenderer.color = new Color(1f, elapsedTime, elapsedTime);
            yield return null;
        }

        //피부 컬러 원복
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * 2f;
            _bodyRenderer.color = new Color(1f, elapsedTime, elapsedTime);
            yield return null;
        }
        
        _bodyRenderer.color = Color.white;
        
        StopCoroutine(_skinEffectCo);
        _skinEffectCo = null;
    }
    
}
