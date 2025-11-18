using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubweaponForge_Temp : MonoBehaviour
{
    [SerializeField] protected ForgeCanvas forge;
    
    [SerializeField] protected ItemButton defaultItemButton;

    private void OnEnable()
    {
        forge.SetCurParts(Parts_kr.보조무기);
        StartCoroutine(PreviewCoroutine());
    }

    private IEnumerator PreviewCoroutine()
    {
        yield return WaitCache.GetWait(0.05f);
        defaultItemButton.Tap_ItemButton();
    }
    
}
