using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class NewGameStart : BaseUI
{
    [SerializeField] private SceneReference inGameScene;
    
    [Header("0: HAIR, 1: BODY, 2: SHIRT, 3: WEAPON")]
    [SerializeField] private List<Image> presets;

    [Inject] private DataManager dataManager;
    
    private Button createButton;
    
    protected void Start()
    {
        Bind();
        ButtonAddListener();
    }
    private void OnValidate()
    {
        if (presets.Count <= 0)
        {
            Debug.LogError($"프리셋 이미지를 넣어주세요. -{gameObject.name}-");
        }
    }
    
    private void Bind()
    {
        createButton = GetUI<Button>("CreateButton"); 
    }

    private void ButtonAddListener()
    {
        createButton.onClick.AddListener(PlayerCreate);
    }

    /// <summary>
    /// 플레이어 생성
    /// </summary>
    private void PlayerCreate()
    {
        dataManager.SavePresetData(presets); 
        SceneManager.LoadScene(inGameScene.Name);
    }
    
    /// <summary>
    /// 캐릭터 프리셋 변경
    /// </summary>
    /// <param name="preset">변경할 부위</param>
    /// <param name="presetSprite">변경할 이미지</param>
    public void ChangePreset(CharacterPresetType preset, Sprite presetSprite)
    {
        presets[(int)preset].sprite = presetSprite;
    }
    
}
