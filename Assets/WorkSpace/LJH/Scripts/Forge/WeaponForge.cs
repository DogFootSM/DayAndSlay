using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class WeaponForge : Forge
{
    [Inject(Id = "WeaponForge")]
    GameObject forgeUi;


    public override void UiOnOffMethod(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            forgeUi.SetActive(!forgeUi.activeSelf);
        }
    }
}
