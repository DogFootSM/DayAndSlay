using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorForge_Temp : MonoBehaviour
{
    [SerializeField] private ForgeCanvas forge;
    [SerializeField] private Button defaultButton;
    private void OnEnable()
    {
        forge.SetCurParts(Parts_kr.≈ı±∏);
        defaultButton.onClick.Invoke();
    }

}
