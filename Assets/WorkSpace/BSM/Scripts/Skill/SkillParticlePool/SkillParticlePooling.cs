using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillParticlePooling : MonoBehaviour
{
    public static SkillParticlePooling Instance;
    
    private Dictionary<string, Queue<GameObject>> skillParticlePool = new Dictionary<string, Queue<GameObject>>();
    private GameObject particleObject;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 초기 게임 시작 시 스킬 이펙트 풀링 추가
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="skillParticle"></param>
    public void InstantiateSkillParticlePool(string skillID, GameObject skillParticle)
    {
        if (!skillParticlePool.ContainsKey(skillID))
        {
            skillParticlePool.Add(skillID, new Queue<GameObject>());

            GameObject instantiate = GameObject.Instantiate(skillParticle, Vector3.zero, Quaternion.identity, transform);
            
            skillParticlePool[skillID].Enqueue(instantiate);
            
            instantiate.SetActive(false);
        }
    }
    
    /// <summary>
    /// 파티클 풀에서 스킬 이펙트를 꺼내옴
    /// </summary>
    /// <param name="effectId">스킬 아이디 값을 기준으로 오브젝트를 검색</param>
    /// <param name="skillParticle">풀에 이펙트가 없을 경우 생성</param>
    /// <returns></returns>
    public GameObject GetSkillPool(string effectId, GameObject skillParticle)
    {
        if (skillParticlePool.ContainsKey(effectId))
        {
            if (skillParticlePool[effectId].Count > 0)
            {
                particleObject = skillParticlePool[effectId].Dequeue(); 
            }
            else
            {
                particleObject = Instantiate(skillParticle); 
            }
        }
        else
        {
            skillParticlePool.Add(effectId, new Queue<GameObject>());
            
            particleObject = Instantiate(skillParticle);
        }
        
        return particleObject;
    }

    /// <summary>
    /// 파티클이 중지될 경우 풀에 파티클 반납
    /// </summary>
    /// <param name="effectId">반납할 스킬의 아이디</param>
    /// <param name="skillParticle">반납할 파티클 오브젝트</param>
    public void ReturnSkillParticlePool(string effectId, GameObject skillParticle)
    {
        skillParticlePool[effectId].Enqueue(skillParticle);
        skillParticle.transform.SetParent(transform);
        skillParticle.SetActive(false); 
    } 
}
