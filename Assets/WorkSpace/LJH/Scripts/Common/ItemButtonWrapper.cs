using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonWrapper
{
    public Button button;
    public WeaponType weaponType;
    public SubWeaponType subWeaponType;

    public ItemButtonWrapper(Button button, WeaponType weaponType)
    {
        this.button = button;
        this.weaponType = weaponType;
    }
    public ItemButtonWrapper(Button button, SubWeaponType subWeaponType)
    {
        this.button = button;
        this.subWeaponType = subWeaponType;
    }
}
