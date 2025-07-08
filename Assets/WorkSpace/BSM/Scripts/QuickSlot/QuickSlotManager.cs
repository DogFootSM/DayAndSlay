using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotManager : MonoBehaviour
{
    [SerializeField] private QuickSlotRegister quickSlotRegister;
    [SerializeField] private SkillTreePreview skillTreePreview;
    
    public static QuickSlotManager Instance;
    private SkillNode selectedSkillNode;

    public SkillNode SelectedSkillNode
    {
        get => selectedSkillNode;
        set => selectedSkillNode = value;
    }

    private CharacterWeaponType curWeaponType;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } 
        //TODO: 퀵슬롯 JSON 저장 필요 
        Init();
    }

    private void Init()
    {
        //퀵슬롯 데이터 초기화
        foreach (CharacterWeaponType weaponType in Enum.GetValues(typeof(CharacterWeaponType)))
        {
            QuickSlotData.WeaponQuickSlotDict[weaponType] = new Dictionary<QuickSlotType, SkillNode>();
        }
    }
    
    /// <summary>
    /// 현재 무기 타입 업데이트
    /// </summary>
    /// <param name="weaponType"></param>
    public void UpdateWeaponType(CharacterWeaponType weaponType)
    {
        curWeaponType = weaponType;
    }
    
    /// <summary>
    /// 스킬 설명 등 선택한 스킬 상세 정보 UI 업데이트
    /// </summary>
    public void UpdateSkillPreviewTab(SkillNode skillNode)
    {
        skillTreePreview.UpdateSkillPreview(skillNode); 
    }
    
    /// <summary>
    /// 퀵슬롯 등록 캔버스 활성화 요청
    /// </summary>
    public void OpenRegisterCanvas()
    {
        quickSlotRegister.OpenRegisterCanvas();
    }

    /// <summary>
    /// 선택한 스킬 등록 진행
    /// </summary>
    /// <returns></returns>
    public void RegisteredSkillNode(QuickSlotType quickSlotType)
    {
        quickSlotRegister.RegisterSkillNode(curWeaponType, quickSlotType, selectedSkillNode);
    }
    
}
