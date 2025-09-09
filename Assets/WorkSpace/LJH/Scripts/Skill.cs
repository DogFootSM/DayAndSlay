using System;
using System.Collections;
using UnityEngine;

public enum PlayerSkillType
{
    LINEAR,
    AOE,
}
public class Skill : MonoBehaviour
{
    [Header("스킬 타입")]
    [SerializeField] private PlayerSkillType type;
    
    [Header("스킬의 범위")]
    [Header("range : 캐릭터와의 거리")]
    [SerializeField] private float range;
    [Header("radius : 캐릭터 기준 원형 범위")]
    [SerializeField] private float radius;

    [Header("이펙트")]
    [SerializeField] ParticleSystem particle;

    [Header("지연 시간")]
    [SerializeField] private float delay;

    [Header("지속 시간")]
    [SerializeField] private float duration;
    
    //임시용 플레이어 변수
    [Header("플레이어")]
    [SerializeField] private PlayerContext player;
    private Vector3 playerPos;
    private void Start()
    {
        playerPos = player.transform.position;
        
        ParticleSystem.MainModule main = particle.main;
        main.startLifetime = duration;

        if (type == PlayerSkillType.LINEAR)
        {
            radius = 1f;
        }
    }
    /// <summary>
    /// 스킬 사용 메서드 - 해당 메서드 호출하면 됨
    /// </summary>
    public void UseSkill()
    {
        switch (type)
        {
            case PlayerSkillType.LINEAR:
                //정면으로 range 만큼 발사
                StartCoroutine(DelayCoroutine());
                break;
            
            case PlayerSkillType.AOE:
                //range 만큼 떨어진 위치에서 radius범위로 스킬 사용
                StartCoroutine(DelayCoroutine());
                break;
        }
    }

    private IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(delay);
        
        particle.Play();
    }
    
    
}
