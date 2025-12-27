using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyEffect : MonoBehaviour
{  
    [Header("Move Settings")]
    public float moveSpeed = 1f;      // 위로 올라가는 속도
    public float lifeTime = 1f;       // 총 지속 시간

    private Vector3 startPos = new Vector3(0.5f, 0.15f, 0);
    private TextMeshProUGUI text;     // 텍스트 컴포넌트
    private Color originalColor;      // 원래 색상값
    private float timer = 0f;

    private Coroutine co;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        originalColor = text.color;
    }

    private void OnEnable()
    {
        // 기존 코루틴이 있으면 정지
        if (co != null)
            StopCoroutine(co);

        // 초기값 세팅
        text.color = originalColor;

        // 시작
        co = StartCoroutine(FloatingRoutine());
    }

    private IEnumerator FloatingRoutine()
    {
        float timer = 0f;

        while (timer < lifeTime)
        {
            timer += Time.deltaTime;

            // 위로 이동
            transform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;

            // 점점 투명해지게
            float alpha = Mathf.Lerp(originalColor.a, 0, timer / lifeTime);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null;
        }

        // 3) 자동 비활성 (풀링용)
        gameObject.SetActive(false);
    }
}
