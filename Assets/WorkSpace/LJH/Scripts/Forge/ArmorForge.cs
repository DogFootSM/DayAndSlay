using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ArmorForge : Forge
{
    [Inject(Id = "ArmorForge")]
    GameObject forgeUi;

    public override void UiOnOffMethod(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            forgeUi.SetActive(!forgeUi.activeSelf);
        }
    }
}
