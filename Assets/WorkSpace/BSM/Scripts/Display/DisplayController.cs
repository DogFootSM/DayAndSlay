using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DisplayController : MonoBehaviour
{
    [Inject] private DataManager dataManager;
    
    [SerializeField] private List<Toggle> windowModeToggles;
    [SerializeField] private List<Toggle> mouseLockToggles;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    
    
    private void Start()
    {
        //Windowed - 창모드
        //Exclusive Fullscreen - 전체 화면
        //Fullscreen Window - 테두리 없는 창모드
        
        //화면 해상도 설정 드롭다운은 전체화면일 때 비활성화
        
    }
}
