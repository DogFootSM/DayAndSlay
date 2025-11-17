using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextPool : MonoBehaviour
{
    [SerializeField] private GameObject _damageTextPrefab;

    public static DamageTextPool Instance;

    private Queue<GameObject> _damageTextPool = new Queue<GameObject>();
    
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
    /// 데미지 텍스트 풀 초기화
    /// </summary>
    private void InitPool()
    {
        for (int i = 0; i < 30; i++)
        {
            GameObject instance = Instantiate(_damageTextPrefab, transform);
            _damageTextPool.Enqueue(instance);
            instance.SetActive(false); 
        } 
    }

    /// <summary>
    /// 데미지 텍스트를 꺼내옴
    /// </summary>
    /// <returns></returns>
    public GameObject GetDamageTextInPool()
    {
        GameObject instance = null;
        
        if (_damageTextPool.Count > 0)
        {
            instance = _damageTextPool.Dequeue();
        }
        else
        {
            instance = Instantiate(_damageTextPrefab, transform);
        }

        return instance;
    }

    /// <summary>
    /// 데미지 텍스트 풀에 반납
    /// </summary>
    /// <param name="instance">반납할 텍스트 오브젝트</param>
    public void ReturnDamageTextInPool(GameObject instance)
    {
        instance.SetActive(false);
        instance.transform.SetParent(transform);
        _damageTextPool.Enqueue(instance);
    }
    
}
