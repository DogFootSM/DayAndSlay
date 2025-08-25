using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotWaitUse : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private RectTransform rectTransform;
    
    private int useAnimHash;

    private void Awake()
    {
        useAnimHash = Animator.StringToHash("WaitUse");
    }

    /// <summary>
    /// 스킬 사용 대기 UI 이펙트 애니메이션 재생
    /// </summary>
    public void PlayAnimation()
    {
        animator.SetBool(useAnimHash, true);
    }

    /// <summary>
    /// 스킬 사용 대기 UI 이펙트 애니메이션 재생
    /// </summary>
    public void StopAnimation()
    {
        animator.SetBool(useAnimHash, false);
    }

    /// <summary>
    /// 스킬 사용 대기 이펙트 포지션 초기화
    /// </summary>
    public void UpdateRectTransform()
    {
        rectTransform.anchoredPosition = Vector2.zero;
    }
}
