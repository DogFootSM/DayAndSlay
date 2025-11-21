using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class CharacterCreate : BaseUI
{
    [SerializeField] private SceneReference inGameScene;
    [SerializeField] private CanvasManager canvasManager;
    [Header("0: HAIR, 1: BODY, 2: SHIRT, 3: WEAPON")]
    [SerializeField] private List<Image> presets;

    [Inject] private DataManager dataManager;
   
    private Button createButton;
    private CharacterWeaponType curWeaponType; 
     
    
    protected void Start()
    {
        ProjectContext.Instance.Container.Inject(this);
        
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
        //현재 프리셋, 무기 타입 저장
        dataManager.SavePresetData(presets, (int)curWeaponType, (int)WeaponTierType.TIER_1_1); 
        dataManager.CreateDataUpdate();
        canvasManager.OnActiveLoadingCanvas(inGameScene); 
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

    /// <summary>
    /// 캐릭터 무기 타입 변경
    /// </summary>
    /// <param name="weaponType">기본은 활 무기로 설정</param>
    public void ChangeWeapon(CharacterWeaponType weaponType = CharacterWeaponType.BOW)
    {
        curWeaponType = weaponType;
    }
    
}
