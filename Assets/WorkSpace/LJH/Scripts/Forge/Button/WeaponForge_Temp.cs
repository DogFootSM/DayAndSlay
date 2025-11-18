using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponForge_Temp : MonoBehaviour
{
    [SerializeField] protected ForgeCanvas forge;
    
    [SerializeField] protected ItemButton defaultItemButton;

    private void OnEnable()
    {
        forge.SetCurParts(Parts_kr.¹«±â);
        StartCoroutine(PreviewCoroutine());
    }

    private IEnumerator PreviewCoroutine()
    {
        yield return WaitCache.GetWait(0.05f);
        defaultItemButton.Tap_ItemButton();
    }

}
