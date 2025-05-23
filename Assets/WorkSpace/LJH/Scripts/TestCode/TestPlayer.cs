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

    GameObject interactObj;
    Table table;

    private void Start()
    {
        table = GetComponent<Table>();
    }
    private void Update()
    {
        PlayerMove();
        TakeInteraction();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        interactObj = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        interactObj = null;
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

    void TakeInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E) &&
            interactObj != null &&
            interactObj.TryGetComponent(out IInteractionStore interactable))
        {
            interactable.Interaction();
        }
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
