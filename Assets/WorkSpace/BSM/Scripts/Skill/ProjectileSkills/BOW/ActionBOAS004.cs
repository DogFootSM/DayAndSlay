using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBOAS004 : MonoBehaviour
{
    private LayerMask monsterLayer;
    
    private int detectedCount;
    private int hitCount;
    private float damage;
    
    private void Awake()
    {
        monsterLayer = LayerMask.GetMask("Monster");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((1 << other.gameObject.layer & monsterLayer) != 0)
        { 
            //타격할 수 있는 몬스터 개수가 남아있는 상태
            if (detectedCount > 0)
            {
                for (int i = 0; i < hitCount; i++)
                {
                    //몬스터 Hit 호출
                    other.gameObject.GetComponent<IEffectReceiver>().TakeDamage(damage);
                } 
                
                //타격했으면 감지 개수 감소
                detectedCount--;
            }
            
            //감지 개수가 0 일 경우 발사체 파티클 정지
            if (detectedCount == 0)
            {
                gameObject.GetComponent<ParticleSystem>().Stop();
            } 
        }
    }
    
    /// <summary>
    /// 해당 스킬의 데이터 설정
    /// </summary>
    /// <param name="detectedCount">타격할 수 있는 몬스터 횟수</param>
    /// <param name="hitCount">몬스터 한 마리당 타격 횟수</param>
    /// <param name="damage">현재 스킬의 데미지</param>
    public void SetSkillData(int detectedCount, int hitCount, float damage)
    {
        this.detectedCount = detectedCount;
        this.hitCount = hitCount;
        this.damage = damage;
    } 
}
