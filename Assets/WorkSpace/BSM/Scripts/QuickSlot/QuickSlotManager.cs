using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotManager : MonoBehaviour
{
    [SerializeField] private SkillTreeUI skillTreeUI;
    
    public static QuickSlotManager Instance;
     
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } 
    }

    /// <summary>
    /// 선택한 스킬을 UI에 전달
    /// </summary>
    /// <param name="skillNode"></param>
    public void NotifySkillPreview(SkillNode skillNode)
    {
        skillTreeUI.UpdateSkillPreview(skillNode);
    }

    /// <summary>
    /// 스킬창 종료 시 프리뷰 종료
    /// </summary>
    public void CloseSkillPreview()
    {
        skillTreeUI.CloseSkillPreview();
    }
    
}
