using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectPool : MonoBehaviour
{
    [SerializeField] private GameObject _hitEffectPrefab;

    public static HitEffectPool Instance;

    private Queue<GameObject> _hitEffectPool = new Queue<GameObject>();
    
    private void Awake()
    {
        InitSingleton();
        InitPool();
    }

    private void InitSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// 히트이미지 풀 초기화
    /// </summary>
    private void InitPool()
    {
        for (int i = 0; i < 30; i++)
        {
            GameObject instance = Instantiate(_hitEffectPrefab, transform);
            _hitEffectPool.Enqueue(instance);
            instance.SetActive(false); 
        } 
    }

    /// <summary>
    /// 히트이펙트 꺼냄
    /// </summary>
    /// <returns></returns>
    public GameObject GetHitEffectInPool()
    {
        GameObject instance = null;
        
        if (_hitEffectPool.Count > 0)
        {
            instance = _hitEffectPool.Dequeue();
        }
        else
        {
            instance = Instantiate(_hitEffectPrefab, transform);
        }

        return instance;
    }

    /// <summary>
    /// 히트이펙트풀 반납
    /// </summary>
    /// <param name="instance"></param>
    public void ReturnHitEffectInPool(GameObject instance)
    {
        instance.SetActive(false);
        instance.transform.SetParent(transform);
        _hitEffectPool.Enqueue(instance);
    }
    
}

