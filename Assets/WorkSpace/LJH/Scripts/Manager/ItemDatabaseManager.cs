using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabaseManager : MonoBehaviour
{
    public static ItemDatabaseManager instance;

    [SerializeField] private ItemDatabase itemDatabase;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        ItemData foundItem = itemDatabase.GetItemByID(0);
        if (foundItem != null)
        {
            Debug.Log($"찾은 아이템: {foundItem.Name}");
        }
    }
}