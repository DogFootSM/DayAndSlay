using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ArmorForge : Forge, IInteractionStore
{
    [Inject(Id = "ArmorForge")]
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
