using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    [SerializeField] private Button audioTabButton;
    [SerializeField] private Button videoTabButton;
    
    [SerializedDictionary] [SerializeField]
    private SerializedDictionary<Setting, GameObject> settingPanels;

    private void Awake()
    {
        audioTabButton.onClick.AddListener(() => OpenSettingPanel(Setting.AUDIO));
        videoTabButton.onClick.AddListener(() => OpenSettingPanel(Setting.VIDEO));
    }

    private void OnEnable()
    {
        OpenSettingPanel(Setting.AUDIO);
    }

    /// <summary>
    /// 설정창 탭 전환
    /// </summary>
    /// <param name="setting">활성화 할 설정 패널</param>
    private void OpenSettingPanel(Setting setting)
    {
        foreach (KeyValuePair<Setting, GameObject> pair in settingPanels)
        {
            if (pair.Key == setting)
            {
                pair.Value.SetActive(true);
            }
            else
            {
                pair.Value.SetActive(false);
            } 
        } 
    }
    
}
