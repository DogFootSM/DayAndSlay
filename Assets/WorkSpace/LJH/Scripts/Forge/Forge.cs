using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forge : MonoBehaviour
{
    [SerializeField] private GameObject forgeUi;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        UiOnOffMethod(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        UiOnOffMethod(collision);
    }

    public virtual void UiOnOffMethod(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            forgeUi.SetActive(!forgeUi.activeSelf);
        }
    }
}
