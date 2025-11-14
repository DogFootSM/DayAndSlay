using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class WeaponForge : InteractableObj
{
    [Inject(Id = "WeaponForge")]
    GameObject forgeUi;

    [Inject(Id = "PopUp")]
    GameObject popUp;
    
    [SerializeField] private SystemWindowController controller;

    public override void Interaction()
    {
        Debug.Log("웨펀포지 열기 실행");
        popUp.SetActive(false);
        forgeUi.SetActive(!forgeUi.activeSelf);
        controller.OpenSystemWindow(SystemType.WEAPON);
    }

    public override void UiOnOffMethod(Collision2D collision)
    {
        popUp.GetComponent<PopUp>().objName = "무기 제작대";
        popUp.SetActive(!popUp.gameObject.activeSelf);
    }
}
