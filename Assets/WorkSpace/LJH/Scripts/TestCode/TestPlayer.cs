using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestPlayer : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public List<ItemData> inventories = new List<ItemData>();

    private GameObject interactObj;

    private Item haveItem;

    [SerializeField] float moveSpeed;
    [SerializeField] public ItemData choiceItem;
    
    private void Update()
    {
        PlayerMove();
        TakeInteraction();
        //for Test
        ItemPick();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        interactObj = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        interactObj = null;
    }

    void ItemPick()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("���̽� �����ۿ� ������ ����");
            choiceItem = inventories[0];
        }
    }

    /*
    void TakeInteraction2()
    {
        if (Input.GetKeyDown(KeyCode.E) &&
            interactObj != null &&
            interactObj.TryGetComponent(out IInteractionStoreScene interactable))
        {
            switch (interactable)
            {
                case Table table:
                    //table.TakeItem(choiceItem);
                    break;

                default :
                    interactable.Interaction();
                    break;
            }
        }
    }
    */
    void TakeInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactObj != null && interactObj.GetComponent<InteractableObj>())
        {
            interactObj.GetComponent<InteractableObj>().Interaction();
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
