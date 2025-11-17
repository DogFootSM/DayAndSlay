using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _damageText;
    [SerializeField] private Animator _damageAnimator;
    
    [Header("몬스터, 플레이어 텍스트 메테리얼")]
    [SerializedDictionary("Type", "Material")]
    [SerializeField] private SerializedDictionary<DamageTextType, Material> _damageTextMaterials; 
    
    private int damageAnimHash= Animator.StringToHash("TakeDamage");
    private DamageTextPool _damageTextPool => DamageTextPool.Instance;
    
    private void OnEnable()
    {
        _damageAnimator.Play(damageAnimHash);
    }

    /// <summary>
    /// 피해 입은 데미지를 설정
    /// </summary>
    /// <param name="text"></param>
    public void SetDamageText(string text, DamageTextType type)
    {
        _damageText.fontMaterial = _damageTextMaterials[type];
        _damageText.text = text;
    }

    /// <summary>
    /// 애니메이션 이벤트
    /// 애니메이션 종료 시 텍스트 풀에 자신을 반납
    /// </summary>
    public void DisableDamageText()
    {
        _damageTextPool.ReturnDamageTextInPool(gameObject);
    }
    
}
