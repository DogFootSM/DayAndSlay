using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponForge_Temp : MonoBehaviour
{
    [SerializeField] private ForgeCanvas forge;
    private void OnEnable()
    {
        forge.SetCurParts(Parts_kr.¹«±â);
    }
}
