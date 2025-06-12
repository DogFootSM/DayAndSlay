using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillTreeUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> skillTabs;
    [SerializeField] private GameObject skillSetPrefab;

    private List<SkillNodeButton> skillNodeButtons = new();
    public UnityAction<WeaponType> OnChangedSkillTab;
     
    private void OnEnable()
    {
        OnChangedSkillTab += SwitchSkillTabByWeapon;
    }

    private void OnDisable()
    {
        OnChangedSkillTab -= SwitchSkillTabByWeapon;
    }

    /// <summary>
    /// 현재 무기에 따른 스킬 화면 노출
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
            if(!skillNode.ContainsKey((WeaponType)i)) continue;
            
            int skillCount = skillNode[(WeaponType)i].Count;

            for (int j = 0; j < skillCount; j++)
            {
                GameObject skillInstance = Instantiate(skillSetPrefab, skillTabs[i].transform.GetChild(0).GetChild(0).transform); 
                
                SkillNodeButton skillNodeButton = skillInstance.GetComponent<SkillNodeButton>();
                skillNodeButton.CurSkillNode = skillNode[(WeaponType)i][j]; 
                skillNodeButtons.Add(skillNodeButton);
            } 
        }  
    }
    
    /// <summary>
    /// 스킬 포인트에 따른 전체 노드 버튼 업데이트
    /// </summary>
    /// <param name="point">현재 보유중인 스킬 포인트</param>
    public void UpdateAllNodeButtonsWithPoint(int point)
    { 
        foreach (SkillNodeButton skillNodeButton in skillNodeButtons)
        {
            skillNodeButton.UpdateSkillButtonState(point);
        } 
    }
    
}
