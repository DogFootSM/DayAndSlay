using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillParticlePooling : MonoBehaviour
{
    public static SkillParticlePooling Instance;

    private Dictionary<string, Queue<GameObject>> skillParticlePool = new Dictionary<string, Queue<GameObject>>();
    
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
    
    
    public void ReturnSkillPool(string skillID, GameObject skillParticle)
    {
        if (skillParticlePool.ContainsKey(skillID))
        {
            if (skillParticlePool[skillID].Count > 0)
            {
                
            }
        }
        
    }
    
}
