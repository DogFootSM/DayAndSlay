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

    DictList<Button> tabButtonListDict = new DictList<Button>();
    DictList<Button> typeButtonListDict = new DictList<Button>();
    DictList<Button> itemButtonListDict = new DictList<Button>();


    private void Start()
    {
    }


}

