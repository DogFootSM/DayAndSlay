using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{

    private Dictionary<Type_Weapon, ItemData> weaponDict = new Dictionary<Type_Weapon, ItemData>();
    private Dictionary<Type_Armor, InventorySlot> slotDict = new Dictionary<Type_Armor, InventorySlot>();
    
    public void ChangeEquipment()
    {
        
    }
    
}
