using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class SavePromptUI : MonoBehaviour
{ 
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private Button saveConfirmButton;
    [SerializeField] private Button saveCancelButton;
    [SerializeField] private GameSaveHandler saveHandler;
    
    [SerializeField] private DayManager dayManager;
    private GameManager gameManager => GameManager.Instance;
    
    private readonly string[] saveAlertText =
    {
        "낮잠을 주무시겠습니까?\n낮잠을 자면 게임이 저장되며, \n밤에 잠을 잘 수 없습니다.",
        "잠을 주무시겠습니까?\n 잠을 자면 게임이 저장되며, \n아침이 됩니다."
    };

    private void Awake()
    {
        saveConfirmButton.onClick.AddListener(() =>
        {
            saveHandler.GameSaveProcess();
            gameObject.SetActive(false);
        });
        saveCancelButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    private void OnEnable()
    { 
        //해당 부분 수정함 _ LJH
        //OnChangeSaveAlertText(gameManager.CurDayState);
        
        //DayManager에게서 낮,밤을 따와서 인식
        OnChangeSaveAlertText((int)dayManager.GetDayOrNight());
    }
 
    /// <summary>
    /// 현재 낮, 밤 상태에 따른 저장 얼럿 문구 변경
    /// </summary>
    /// <param name="dayState">시간의 흐름에 따라 변화된 현재 낮과밤 상태 0 = 낮, 밤 = 1</param>
    private void OnChangeSaveAlertText(int dayState)
    {
        alertText.text = saveAlertText[dayState];
    }
}
