using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeMaterial : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI needItemCount;
    
    public void SetMaterialData(ItemData itemData)
    {
        itemImage.sprite = itemData.ItemImage;
        itemName.text = itemData.Name;
    }
    
}
