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

    public ItemData GetItemByID(int ID) => itemDatabase.GetItemByID(ID);

    public ItemData GetItemByName(string name) => GetItemByID(itemDatabase.GetItemByName(name));

    public ItemRecipe GetRecipeByID(int Id) => itemDatabase.GetRecipeByID(Id);
}