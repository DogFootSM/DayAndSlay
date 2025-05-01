using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameStart : BaseUI
{
    [SerializeField] private SceneReference inGameScene;
    
    private Image hiarImage;
    private Button createButton;

    private void Start()
    {
        Bind();
        ButtonAddListener();
    }

    private void Bind()
    {
        createButton = GetUI<Button>("CreateButton");
    }

    private void ButtonAddListener()
    {
        createButton.onClick.AddListener(ButtonAddListener);
    }

    /// <summary>
    /// 플레이어 생성
    /// </summary>
    private void PlayerCreate()
    {
        
    }
    
}
