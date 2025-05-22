using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class WeaponForge : Forge, IInteractionStore
{
    [Inject(Id = "WeaponForge")]
    GameObject forgeUi;

    public void Interaction()
    {
        forgeUi.SetActive(!forgeUi.activeSelf);
    }

    public override void UiOnOffMethod(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Interaction();
        }
    }
}
