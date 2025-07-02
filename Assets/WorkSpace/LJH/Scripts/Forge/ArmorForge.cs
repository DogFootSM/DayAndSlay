using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ArmorForge : InteractableObj
{
    [Inject(Id = "ArmorForge")]
    GameObject forgeUi;

    [Inject(Id = "PopUp")]
    GameObject popUp;

    public override void Interaction(ItemData dummy)
    {
        popUp.SetActive(false);
        forgeUi.SetActive(!forgeUi.activeSelf);
    }

    public override void UiOnOffMethod(Collision2D collision)
    {
        popUp.GetComponent<PopUp>().objName = "방어구 제작대";
        popUp.SetActive(!popUp.gameObject.activeSelf);
    }
}
