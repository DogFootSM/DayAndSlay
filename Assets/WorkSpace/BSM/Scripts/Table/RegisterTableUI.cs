using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class RegisterTableUI : MonoBehaviour
{
    [SerializeField] private Image registerItemImage;
    [SerializeField] private TextMeshProUGUI registerItemText;

    public UnityAction<ItemData> OnRegisterItemEvents;

    private void OnEnable()
    {
        OnRegisterItemEvents += OnRegisterItem;
    }

    private void OnDisable()
    {
        OnRegisterItemEvents -= OnRegisterItem;
    }

    private void OnRegisterItem(ItemData item)
    {
        //전달 받은 아이템 데이터로 정보 갱신
    }

    private void OnClickRegisterButton()
    {
        //등록 버튼 클릭
    }

}
