using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateButton : MonoBehaviour
{
    Button thisButton;
    public ItemData curSelectedItem;

    private void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(() => CreateItem());
    }
    public void CreateItem()
    {
        if(curSelectedItem == null)
        {
            Debug.Log("선택된 아이템이 없습니다 > 아예 버튼 비활성화가 나을지?");
        }

        Debug.Log($"{ curSelectedItem} 제작됨");
    }
}
