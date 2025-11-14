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

    [SerializeField] private SystemWindowController controller;
    
    public override void Interaction()
    {
        popUp.SetActive(false);
        forgeUi.SetActive(!forgeUi.activeSelf);
        controller.OpenSystemWindow(SystemType.HELMET);
    }

    public override void UiOnOffMethod(Collision2D collision)
    {
        popUp.GetComponent<PopUp>().objName = "방어구 제작대";
        popUp.SetActive(!popUp.gameObject.activeSelf);
    }
}
