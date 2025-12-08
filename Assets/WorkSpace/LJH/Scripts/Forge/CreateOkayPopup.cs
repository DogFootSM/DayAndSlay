using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateOkayPopup : MonoBehaviour, IPointerClickHandler
{
    private ItemData curItemData;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI text;
    public void SetCurItemInfo(ItemData item)
    {
        itemImage.sprite = item.ItemImage;
        text.text = $"{item.name} \n 제작 완료";
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.SetActive(false);
    }
    
    
}
