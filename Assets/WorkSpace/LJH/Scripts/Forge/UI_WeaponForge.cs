using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponForge : BaseUI
{
    //타입 버튼을 누르면 해당 타입에 존재하는 아이템 갯수만큼 버튼이 생기고 버튼의 이름이 그것들로 채워져야함
    //책 우측 탭을 선택하여 메인무기, 서브무기 변경 가능
    // 메인무기를 선택하면 타입에 메인무기 목록이 떠야함
    // 서브무기를 선택하면 타입에 서브무기 목록이 떠야함

    private List<Button> tabButtonList = new List<Button>();
    private List<Button> typeButtonList = new List<Button>();
    private List<Button> itemButtonList = new List<Button>();


    private void Start()
    {
        ButtonDictList<Button> buttonListDict = new ButtonDictList<Button>();
    }


}

public class ButtonDictList<Button>
{
    private List<Button> buttonList = new List<Button>();
    private Dictionary<string, Button> buttonDict = new Dictionary<string, Button>();

    public void Add(string key, Button value)
    {
        buttonList.Add(value);
        buttonDict[key] = value;
    }

    public Button this[int index] => buttonList[index];

    public Button this[string key] => buttonDict[key];

    public int Count => buttonList.Count;
}
