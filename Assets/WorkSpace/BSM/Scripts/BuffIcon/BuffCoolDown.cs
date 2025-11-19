using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffCoolDown : MonoBehaviour
{
    [SerializeField] private Image coolDownImage;
    [SerializeField] private Image buffImage;
    
    /// <summary>
    /// 쿨다운 리셋 UI 진행
    /// </summary>
    /// <param name="coolDownDuration"></param>
    public void ResetCoolDown(float coolDownDuration = 0f)
    {
        coolDownImage.fillAmount = 1f;
        StartCoroutine(ResetCoolDownCoroutine(coolDownDuration));
    }

    private IEnumerator ResetCoolDownCoroutine(float coolDownDuration = 0f)
    {
        float elapsedTime = coolDownDuration;

        while (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            coolDownImage.fillAmount = elapsedTime / coolDownDuration;
            yield return null;
        }
        
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 버프 아이콘 이미지 설정
    /// </summary>
    /// <param name="sprite"></param>
    public void SetIconImage(Sprite sprite)
    {
        buffImage.sprite = sprite;
    }
}
