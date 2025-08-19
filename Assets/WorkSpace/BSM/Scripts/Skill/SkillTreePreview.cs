using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class SkillTreePreview : MonoBehaviour
{
    [SerializeField] private List<GameObject> skillTabs;
    [SerializeField] private GameObject skillSetPrefab;

    [Header("스킬 미리보기")] [SerializeField] private GameObject previewTab;
    [SerializeField] private TextMeshProUGUI previewSkillNameText;
    [SerializeField] private TextMeshProUGUI previewSkillDescriptionText;
    [SerializeField] private TextMeshProUGUI previewSkillPrerequisiteText;
    [SerializeField] private Image previewSkillIcon;
    [SerializeField] private Button previewRegisterButton;
    
    private List<SkillSet> skillNodeButtons = new();
    public UnityAction<WeaponType> OnChangedSkillTab;

    private QuickSlotManager quickSlotManager => QuickSlotManager.Instance;
    private SkillNode selectedSkillNode;

    private void Start()
    {
        previewRegisterButton.onClick.AddListener(() =>
        {
            quickSlotManager.OpenRegisterCanvas();
            quickSlotManager.SelectedSkillNode = selectedSkillNode;
        });
    }

    private void OnEnable()
    {
        OnChangedSkillTab += SwitchSkillTabByWeapon;
    }

    private void OnDisable()
    {
        OnChangedSkillTab -= SwitchSkillTabByWeapon;
    }

    /// <summary>
    /// 현재 무기에 따른 스킬 트리 UI 노출
    /// </summary>
    /// <param name="weaponType"></param>
    private void SwitchSkillTabByWeapon(WeaponType weaponType)
    {
        skillTabs[0].SetActive(weaponType == WeaponType.BOW);
        skillTabs[1].SetActive(weaponType == WeaponType.SHORT_SWORD);
        skillTabs[2].SetActive(weaponType == WeaponType.SPEAR);
        skillTabs[3].SetActive(weaponType == WeaponType.WAND);
        skillTabs[4].SetActive(weaponType == WeaponType.NOT_WEAPON);
    }

    /// <summary>
    /// 캐릭터 스킬 패널창 셋팅
    /// </summary>
    /// <param name="skillNode">각 스킬 데이터를 가지고 있는 스킬 노드 리스트</param>
    public void InstantiateSkillPrefabs(Dictionary<WeaponType, List<SkillNode>> skillNode)
    {
        for (int i = 0; i < (int)CharacterWeaponType.SIZE; i++)
        {
            if (!skillNode.ContainsKey((WeaponType)i)) continue;

            int skillCount = skillNode[(WeaponType)i].Count;

            for (int j = 0; j < skillCount; j++)
            {
                GameObject skillInstance =
                    Instantiate(skillSetPrefab, skillTabs[i].transform.GetChild(0).GetChild(0).transform);

                SkillSet skillSet = skillInstance.GetComponent<SkillSet>();
                skillSet.CurSkillNode = skillNode[(WeaponType)i][j];
                skillSet.UpdateSkillPreviewPrerequisite += UpdatePrerequisiteSkillNode;
                skillNodeButtons.Add(skillSet);
            }
        }
    }

    /// <summary>
    /// 스킬 포인트에 따른 전체 노드 버튼 업데이트
    /// </summary>
    /// <param name="point">현재 보유중인 스킬 포인트</param>
    public void UpdateAllNodeButtonsWithPoint(int point)
    {
        foreach (SkillSet skillNodeButton in skillNodeButtons)
        {
            skillNodeButton.UpdateSkillButtonState(point);
        }
    }

    /// <summary>
    /// 선택한 스킬 미리보기 업데이트
    /// </summary>
    /// <param name="skillNode">현재 선택한 스킬 노드</param>
    public void UpdateSkillPreview(SkillNode skillNode)
    {
        if (!previewTab.activeSelf) previewTab.SetActive(true);
        
        selectedSkillNode = skillNode;
        previewSkillNameText.text = skillNode.skillData.SkillId;
        previewSkillDescriptionText.text = skillNode.skillData.SkillDescription;
        previewSkillIcon.sprite = skillNode.skillData.SkillIcon;
        
        previewRegisterButton.gameObject.SetActive(selectedSkillNode.skillData.IsActive);
        
        previewRegisterButton.interactable = selectedSkillNode.CurSkillLevel > 0;
        UpdatePrerequisiteSkillNode();
    }

    /// <summary>
    /// 선행 스킬 필요 안내 UI 업데이트
    /// </summary>
    /// <param name="skillNode">스킬 등록창에서 선택한 스킬 노드</param>
    public void UpdatePrerequisiteSkillNode(SkillNode skillNode = null)
    {
        if (selectedSkillNode == null)
        {
            selectedSkillNode = skillNode;
        }
        
        int prerequisiteCount = selectedSkillNode.PrerequisiteSkillNode.Where(x => x.CurSkillLevel < 1).Count();
        
        previewSkillPrerequisiteText.gameObject.SetActive(prerequisiteCount != 0);

        if (prerequisiteCount == 0) return;
        
        string prerequisite = "";

        for (int i = 0; i < selectedSkillNode.PrerequisiteSkillNode.Count; i++)
        {
            if (selectedSkillNode.PrerequisiteSkillNode[i].CurSkillLevel < 1)
            {
                prerequisite += selectedSkillNode.PrerequisiteSkillNode[i].skillData.SkillId;
                
                if (i < prerequisiteCount - 1)
                {
                    prerequisite += ", ";
                }
            } 
        } 
        
        prerequisite += " 스킬의 선행 조건 달성이 필요합니다.";
        previewSkillPrerequisiteText.text = prerequisite; 
    }
    
    /// <summary>
    /// 스킬 프리뷰 항목 초기화
    /// </summary>
    public void CloseSkillPreview()
    {
        previewTab.SetActive(false);
        selectedSkillNode = null;
    }
}