using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    private SpriteRenderer sprite;

    public int ItemId;
    
    [Tooltip("전체 왕복에 걸리는 시간 (초)")]
    [SerializeField] private float floatSpeed = 2; 

    [Tooltip("중앙 위치를 기준으로 위아래로 움직이는 최대 거리")]
    [SerializeField] private float floatHeight = 0;
    private Vector3 startPos;
    private Coroutine floatCoroutine;

    // --- 습득 효과 기능 관련 ---
    [Tooltip("아이템 습득 효과가 지속될 시간 (초)")]
    [SerializeField] private float pickupDuration = 0.3f; // 습득 효과 지속 시간

    [SerializeField] private int hpRecoveryAmount;
    public int HpRecoveryAmount => hpRecoveryAmount;

    private IEnumerator FloatRoutine()
    {
        float timer = 0f; 

        while (true)
        {
            timer += Time.deltaTime; 

            float yOffset = Mathf.Sin(timer * floatSpeed) * floatHeight;

            transform.position = startPos + new Vector3(0, yOffset, 0);

            yield return null;
        }
    }

    private void Start()
    {
        startPos = transform.position;

        // 2. 둥실거리는 코루틴을 시작합니다.
        floatCoroutine = StartCoroutine(FloatRoutine());
    }
    
    public void SetRecoveryAmount(int amount) => hpRecoveryAmount = amount;
    
    /// <summary>
    /// 아이템 습득 효과를 시작하는 메서드
    /// </summary>
    /// <param name="targetTransform"></param>
    public void StartPickupEffect(Transform targetTransform) // targetTransform을 받습니다.
    {
        // 1. 둥실거리는 코루틴 중지
        if (floatCoroutine != null)
        {
            StopCoroutine(floatCoroutine);
            floatCoroutine = null;
        }
        
        Vector3 currentPosition = transform.position;
        Vector3 startScale = transform.localScale;
        
        StartCoroutine(PickupEffectCoroutine(currentPosition, startScale, targetTransform));
    }
    
    /// <summary>
    /// 아이템이 캐릭터 위치로 작아지며 빨려 들어가는 코루틴
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="startScale"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    private IEnumerator PickupEffectCoroutine(Vector3 startPos, Vector3 startScale, Transform endTransform) 
    {
        float timeElapsed = 0f;

        while (timeElapsed < pickupDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / pickupDuration; 
            
            Vector3 currentTargetPos = endTransform.position;
            
            transform.position = Vector3.Lerp(startPos, currentTargetPos, t);
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            
            yield return null;
        }

        // 최종 보정
        transform.position = endTransform.position;
        transform.localScale = Vector3.zero;

        // 아이템 획득 로직 호출 및 오브젝트 파괴
        Destroy(gameObject); 
    }
}