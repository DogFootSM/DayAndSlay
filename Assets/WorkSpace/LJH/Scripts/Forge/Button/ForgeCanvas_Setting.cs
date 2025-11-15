using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgeCanvas_Setting : MonoBehaviour
{
    [SerializeField] protected ForgeCanvas forge;
    
    [SerializeField] protected ItemButton defaultItemButton;
    [SerializeField] private Parts_kr parts;

    private void OnEnable()
    {
        forge.SetCurParts(parts);
        StartCoroutine(PreviewCoroutine());
    }

    private IEnumerator PreviewCoroutine()
    {
        yield return WaitCache.GetWait(0.05f);
        defaultItemButton.Tap_ItemButton();
    }
    
}
