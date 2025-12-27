using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ArrowPool : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;

    public static ArrowPool Instance;
    
    private Queue<GameObject> arrowPool = new Queue<GameObject>();
    
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
    /// 활 무기 장착 시 화살 오브젝트 생성
    /// </summary>
    public void SetupArrowPoolOnEquip()
    {
        if (arrowPool.Count < 1)
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject arrowInstance = Instantiate(arrowPrefab, transform.position, Quaternion.identity, transform);
                arrowPool.Enqueue(arrowInstance);
                arrowInstance.SetActive(false);
            } 
        } 
    }

    /// <summary>
    /// 화살 오브젝트를 꺼내 플레이어에게 반환
    /// </summary>
    /// <returns></returns>
    public GameObject GetPoolArrow()
    {
        GameObject poolArrow;
        
        if (arrowPool.Count > 0)
        {
            poolArrow = arrowPool.Dequeue();
        }
        else
        {
            poolArrow = Instantiate(arrowPrefab);
        }

        poolArrow.SetActive(true);
        
        return poolArrow;
    }

    /// <summary>
    /// 화살 사용 후 화살 풀에 반환
    /// </summary>
    /// <param name="arrowObject">반환할 화살 오브젝트</param>
    public void ReturnPoolArrow(GameObject arrowObject)
    {
        arrowPool.Enqueue(arrowObject);
        arrowObject.SetActive(false);
        arrowObject.transform.SetParent(transform);
    }
    
}
