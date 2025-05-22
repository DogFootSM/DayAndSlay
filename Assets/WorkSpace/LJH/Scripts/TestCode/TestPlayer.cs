using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestPlayer : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    [SerializeField] private List<ItemRecipe> recipes = new List<ItemRecipe>();

    [SerializedDictionary]
    public Dictionary<ItemRecipe, bool> recipeOpened = new Dictionary<ItemRecipe, bool>();
    public Dictionary<Ingredient, ItemRecipe> IngredientDic = new Dictionary<Ingredient, ItemRecipe>();

    public Ingredient ingre;
    public ItemRecipe recipe;


    [SerializeField] float moveSpeed;
    private void Update()
    {
        PlayerMove();
    }

    void RecipeOpen()
    {
        //ÃÊ±â °ª
        for (int i = 0; i < recipes.Count; i++)
        {
            recipeOpened.Add(recipes[i], false);
        }
    }

    void GetRecipe()
    {
        IngredientDic.Add(ingre, recipe);
    }


    void PlayerMove()
    {
        if(Input.GetAxis("Horizontal") > 0)
        {
            gameObject.transform.position += new Vector3(moveSpeed,0,0) * Time.deltaTime;
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            gameObject.transform.position += new Vector3(-moveSpeed, 0, 0) * Time.deltaTime;
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            gameObject.transform.position += new Vector3(0, moveSpeed, 0) * Time.deltaTime;
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            gameObject.transform.position += new Vector3(0, -moveSpeed, 0) * Time.deltaTime;
        }
    }

}
