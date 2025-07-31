using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeElement : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

    /// <summary>
    /// 레시피 요소 데이터 설정
    /// </summary>
    /// <param name="itemImange">해당 아이템의 이미지</param>
    /// <param name="itemName">아이템의 이름</param>
    /// <param name="itemDesc">아이템 설명</param>
    public void SetElement(Sprite itemImange, string itemName, string itemDesc)
    {
        itemImage.sprite = itemImange;
        this.itemName.text = itemName;
        itemDescription.text = itemDesc;
    }
    
}
