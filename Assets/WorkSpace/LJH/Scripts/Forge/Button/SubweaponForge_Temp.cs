using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubweaponForge_Temp : WeaponForge_Temp
{
    protected override void OnEnable()
    {
        forge.SetCurParts(Parts_kr.보조무기);
    }
}
