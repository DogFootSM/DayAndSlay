using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Bed : InteractableObj
{
    [Inject(Id = "PopUp")] private GameObject popUp;
    [Inject]SqlManager sqlManager;
    public override void Interaction()
    {
        Debug.Log("침대에 누움");
        //DayManager.instance.Save(sqlManager);
    }

    public override void UiOnOffMethod(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            popUp.GetComponent<PopUp>().objName = "침대";
            popUp.SetActive(!popUp.gameObject.activeSelf);
        }
    }
}
