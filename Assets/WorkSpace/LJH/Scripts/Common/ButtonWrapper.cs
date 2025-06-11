using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonWrapper
{
    public string buttonName;
    public Button button;
    public WeaponType weaponType;
    public SubWeaponType subWeaponType;
    public Parts parts;
    public ItemData itemData;

    public ButtonWrapper(string buttonName, Button button, WeaponType weaponType)
    {
        this.buttonName = buttonName;
        this.button = button;
        this.weaponType = weaponType;
    }
    public ButtonWrapper(string buttonName, Button button, SubWeaponType subWeaponType)
    {
        this.buttonName = buttonName;
        this.button = button;
        this.subWeaponType = subWeaponType;
    }
    public ButtonWrapper(string buttonName, Button button, Parts parts)
    {
        this.buttonName = buttonName;
        this.button = button;
        this.parts = parts;
    }
    public ButtonWrapper(string buttonName, Button button, ItemData itemData)
    {
        this.buttonName = buttonName;
        this.button = button;
        itemData = button.GetComponent<ItemButton>().itemData;
        this.itemData = itemData;
    }
    public ButtonWrapper(string buttonName, Button button)
    {
        this.buttonName = buttonName;
        this.button = button;
    }
}

