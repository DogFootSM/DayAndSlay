using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonWrapper
{
    public Button button;
    public WeaponType weaponType;
    public SubWeaponType subWeaponType;
    public Parts parts;
    public ItemData itemData;

    public ButtonWrapper(Button button, WeaponType weaponType)
    {
        this.button = button;
        this.weaponType = weaponType;
    }
    public ButtonWrapper(Button button, SubWeaponType subWeaponType)
    {
        this.button = button;
        this.subWeaponType = subWeaponType;
    }
    public ButtonWrapper(Button button, Parts parts)
    {
        this.button = button;
        this.parts = parts;
    }
    public ButtonWrapper(Button button, ItemData itemData)
    {
        this.button = button;
        itemData = button.GetComponent<ItemButton>().itemData;
        this.itemData = itemData;
    }
    public ButtonWrapper(Button button)
    {
        this.button = button;
    }
}

