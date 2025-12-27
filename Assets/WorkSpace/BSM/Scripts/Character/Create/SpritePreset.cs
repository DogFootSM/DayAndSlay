using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpritePreset : BaseUI
{
    [SerializeField] private List<Sprite> spriteList;
    
    public CharacterCreate SelectPreset;
    
    [Header("현재 프리셋 부위")]
    public CharacterPresetType CurPresetType;

    [Header("현재 선택한 무기 타입")] 
    public CharacterWeaponType CurWeaponType;
    
    private Sprite curSprite;
    private Button prevButton;
    private Button nextButton;
    private SoundManager soundManager => SoundManager.Instance;
    
    private int presetIndex;
    
    private void Start()
    {
        Bind();
        ButtonAddListener();
        SpriteLoad(); 
        SendSelectedPreset(); 
    }

    /// <summary>
    /// 스프라이트 파일 로드
    /// </summary>
    private void SpriteLoad()
    {
        if (CurPresetType == CharacterPresetType.WEAPON)
        {
            spriteList.Add(Resources.Load<Sprite>($"Preset/{CurPresetType.ToString()}/WEAPON_BOW_TIER_1_1"));
            spriteList.Add(Resources.Load<Sprite>($"Preset/{CurPresetType.ToString()}/WEAPON_SHORT_SWORD_TIER_1_1"));
            spriteList.Add(Resources.Load<Sprite>($"Preset/{CurPresetType.ToString()}/WEAPON_SPEAR_TIER_1_1"));
            spriteList.Add(Resources.Load<Sprite>($"Preset/{CurPresetType.ToString()}/WEAPON_WAND_TIER_1_1"));
        }
        else
        {
            spriteList = Resources.LoadAll<Sprite>($"Preset/{CurPresetType.ToString()}").ToList();
        }
    }
    
    /// <summary>
    /// UI 오브젝트 바인딩
    /// </summary>
    private void Bind()
    {
        prevButton = GetUI<Button>("PrevButton");
        nextButton = GetUI<Button>("NextButton");
    }

    /// <summary>
    /// 버튼 이벤트 등록
    /// </summary>
    private void ButtonAddListener()
    {
        prevButton.onClick.AddListener(PrevPreset);
        nextButton.onClick.AddListener(NextPreset);
    }
    
    /// <summary>
    /// 이전 복장 선택
    /// </summary>
    private void PrevPreset()
    {
        presetIndex--;
        soundManager.PlaySfx(SFXSound.ARROW_BUTTON_SELECT);
        
        if(presetIndex < 0) presetIndex = spriteList.Count - 1;
        SendSelectedPreset(); 
    }

    /// <summary>
    /// 다음 복장 선택
    /// </summary>
    private void NextPreset()
    {
        presetIndex++;
        soundManager.PlaySfx(SFXSound.ARROW_BUTTON_SELECT);
        
        if(presetIndex > spriteList.Count - 1) presetIndex = 0;
        SendSelectedPreset();
    }

    /// <summary>
    /// 선택 프리셋 정보를 보냄
    /// </summary>
    private void SendSelectedPreset()
    {
        curSprite = spriteList[presetIndex]; 
        SelectPreset.ChangePreset(CurPresetType, curSprite);
        
        if (CurPresetType == CharacterPresetType.WEAPON)
        {
            CurWeaponType = (CharacterWeaponType)presetIndex;
            SelectPreset.ChangeWeapon(CurWeaponType);
        }
        
    }
    
    
}
