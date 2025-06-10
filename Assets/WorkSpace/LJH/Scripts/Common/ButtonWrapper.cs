using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeButtonWrapper
{
    public Button button;
    public WeaponType weaponType;
    public SubWeaponType subWeaponType;
    public Parts parts;

    public TypeButtonWrapper(Button button, WeaponType weaponType)
    {
        this.button = button;
        this.weaponType = weaponType;
    }
    public TypeButtonWrapper(Button button, SubWeaponType subWeaponType)
    {
        this.button = button;
        this.subWeaponType = subWeaponType;
    }
    public TypeButtonWrapper(Button button, Parts parts)
    {
        this.button = button;
        this.parts = parts;
    }
}

public class ItemButtonWrapper
{
    public Button button;
    public ItemData itemData;

    public ItemButtonWrapper(Button button)
    {
        this.button = button;
        itemData = button.GetComponent<ItemButton>().itemData;
    }
}
