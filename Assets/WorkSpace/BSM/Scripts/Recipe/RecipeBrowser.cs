using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeBrowser : MonoBehaviour
{
    private ItemDatabaseManager itemDatabase => ItemDatabaseManager.instance;
    
    [SerializeField]
    private List<ItemRecipe> recipeList => itemDatabase.ItemDatabase.recipes;
    
    private Dictionary<string, int> searchRecipeMap = new Dictionary<string, int>();
    private TMP_Dropdown test;
    private void Awake()
    { 
    }
}
