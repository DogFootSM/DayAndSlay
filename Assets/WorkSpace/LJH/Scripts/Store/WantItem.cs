using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WantItem : MonoBehaviour
{
    private TextMeshProUGUI itemName;

    public void Awake()
    {
        itemName = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    public void SetItemName(string itemName) => this.itemName.text = itemName;


}
