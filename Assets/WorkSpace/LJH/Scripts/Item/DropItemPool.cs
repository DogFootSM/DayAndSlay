using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemPool : MonoBehaviour
{
    [SerializeField] GameObject dropItem;
    Queue<GameObject> dropItemPool = new Queue<GameObject>();

    int dropItemPoolSize = 12;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="count">±âº»°ª 12</param>
    public void InitPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(dropItem);
            obj.SetActive(false);
            dropItemPool.Enqueue(obj);
        }
    }

    public GameObject GetPool()
    {
        if (dropItemPool.Count <= 0)
        {
            ExpandPool(dropItemPoolSize);
        }

        GameObject obj = dropItemPool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        dropItemPool.Enqueue(obj);
    }

    public void ExpandPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(dropItem);
            obj.SetActive(false);
            dropItemPool.Enqueue(obj);
        }
    }
}
