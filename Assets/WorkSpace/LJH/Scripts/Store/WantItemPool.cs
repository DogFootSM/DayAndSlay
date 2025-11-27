using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WantItemPool : MonoBehaviour
{
    
    [SerializeField] private GameObject _wantItemPrefab;

    public static WantItemPool Instance;

    private Queue<GameObject> _wantItemPool = new Queue<GameObject>();
    
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
    /// æ∆¿Ã≈€ «Æ √ ±‚»≠
    /// </summary>
    private void InitPool()
    {
        for (int i = 0; i < 30; i++)
        {
            GameObject instance = Instantiate(_wantItemPrefab, transform);
            _wantItemPool.Enqueue(instance);
            instance.SetActive(false); 
        } 
    }

    /// <summary>
    /// æ∆¿Ã≈€ «¡∏Æ∆’ ≤®≥ø
    /// </summary>
    /// <returns></returns>
    public GameObject GetWantItemInPool()
    {
        GameObject instance = null;
        
        if (_wantItemPool.Count > 0)
        {
            instance = _wantItemPool.Dequeue();
        }
        else
        {
            instance = Instantiate(_wantItemPrefab, transform);
        }

        return instance;
    }

    /// <summary>
    /// æ∆¿Ã≈€ «¡∏Æ∆’ π›≥≥
    /// </summary>
    /// <param name="instance"></param>
    public void ReturnWantItemInPool(GameObject instance)
    {
        instance.SetActive(false);
        instance.transform.SetParent(transform);
        _wantItemPool.Enqueue(instance);
    }
}
