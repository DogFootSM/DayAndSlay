using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponForge_Temp : MonoBehaviour
{
    [SerializeField] private ForgeCanvas forge;

    [Header("해당하는 탭 버튼을 할당")]
    [SerializeField] private Button defaultButton;
    private void OnEnable()
    {
        forge.SetCurParts(Parts_kr.무기);
        defaultButton.GetComponent<TapButton>().SetTypeButtons();
    }
}
