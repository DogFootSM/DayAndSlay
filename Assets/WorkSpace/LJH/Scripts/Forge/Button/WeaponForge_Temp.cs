using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponForge_Temp : MonoBehaviour
{
    [SerializeField] protected ForgeCanvas forge;

    protected virtual void OnEnable()
    {
        forge.SetCurParts(Parts_kr.¹«±â);
    }

}
