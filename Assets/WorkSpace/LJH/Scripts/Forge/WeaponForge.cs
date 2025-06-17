using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class WeaponForge : InteractableObj, IInteractionStoreScene
{
    [Inject(Id = "WeaponForge")]
    GameObject forgeUi;

    [Inject(Id = "PopUp")]
    GameObject popUp;

    public override void Interaction()
    {
        popUp.SetActive(false);
        forgeUi.SetActive(!forgeUi.activeSelf);
    }

    public override void UiOnOffMethod(Collision2D collision)
    {
        popUp.GetComponent<PopUp>().objName = "무기 제작대";
        popUp.SetActive(!popUp.gameObject.activeSelf);
    }
}
