using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolDown : MonoBehaviour
{
    [SerializeField] private Image coolDownImage;

    private Coroutine coolDownResetCo;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// 쿨타임 UI 코루틴 호출
    /// </summary>
    /// <param name="skillNode"></param>
    public void SkillSlotUpdateCoolDown(SkillNode skillNode)
    {
        if (coolDownResetCo == null)
        {
            coolDownImage.fillAmount = 1f;
            coolDownResetCo = StartCoroutine(CoolDownResetRoutine(skillNode));
        }
    }

    /// <summary>
    /// 스킬 쿨타임 UI 업데이트 코루틴
    /// </summary>
    /// <param name="skillNode">업데이트에 사용할 스킬의 쿨타임 시간</param>
    /// <returns></returns>
    private IEnumerator CoolDownResetRoutine(SkillNode skillNode)
    {
        float elapsedTime = skillNode.skillData.SkillCooldown;

        while (elapsedTime >= 0)
        {
            elapsedTime -= Time.deltaTime;
            coolDownImage.fillAmount = elapsedTime / skillNode.skillData.SkillCooldown;
            yield return null;
        }

        skillNode.IsCoolDownReset = true;

        if (coolDownResetCo != null)
        {
            StopCoroutine(coolDownResetCo);
            coolDownResetCo = null;
        }
    }
  
    /// <summary>
    /// CoolDown UI 위치 초기화
    /// </summary>
    public void UpdateAnchorPreset()
    {
        rectTransform.anchoredPosition = Vector2.zero; 
    }
    
}