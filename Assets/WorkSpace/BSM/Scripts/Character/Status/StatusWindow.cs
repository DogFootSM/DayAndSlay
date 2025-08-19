using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatusWindow : MonoBehaviour
{ 
    [Header("스탯에 대한 텍스트")]
    [Tooltip("1.힘 / 2.민첩 / 3.지력 / 4.체력 / 5.크리티컬 / 6.물리 공격력 / 7.물리 방어력 / 8.스킬 공격력 / 9.스킬 방어력 / 10.보유 포인트")]
    [SerializeField] private List<TextMeshProUGUI> statsTexts = new List<TextMeshProUGUI>();
    
    [Header("0.힘 증가 버튼 / 1.민첩 증가 버튼 / 2.지력 증가 버튼")]
    [SerializeField] private List<Button> increaseButtons = new List<Button>();

    [SerializeField] private PlayerModel playerModel;

    public UnityAction<int> OnActiveIncreaseButton;
    public UnityAction<PlayerStats> OnChangedAllStats;

    private void OnEnable() => SubscribeEvent();

    private void OnDisable() => UnsubscribeEvent();

    private void Start()
    {
        increaseButtons[0].onClick.AddListener(() => playerModel.AdjustStats(CharacterStatsType.STR));
        increaseButtons[1].onClick.AddListener(() => playerModel.AdjustStats(CharacterStatsType.AGI));
        increaseButtons[2].onClick.AddListener(() => playerModel.AdjustStats(CharacterStatsType.INT));
    }

    /// <summary>
    /// 이벤트 구독
    /// </summary>
    private void SubscribeEvent()
    {
        OnChangedAllStats += ChangedAllStats;
        OnActiveIncreaseButton += ActiveIncreaseButton;
    }

    /// <summary>
    /// 이벤트 구독 해제
    /// </summary>
    private void UnsubscribeEvent()
    {
        OnChangedAllStats -= ChangedAllStats; 
        OnActiveIncreaseButton -= ActiveIncreaseButton;
    }
    
    /// <summary>
    /// 전체 스탯 변경
    /// </summary>
    /// <param name="playerStats">변경된 스탯을 넘겨 받음</param>
    //private void ChangedAllStats(PlayerStats playerStats, ItemData itemData = null)
    private void ChangedAllStats(PlayerStats playerStats)
    { 
        statsTexts[0].text = $"{playerStats.FinalStrength}";
        statsTexts[1].text = $"{playerStats.FinalAgility}";
        statsTexts[2].text = $"{playerStats.FinalIntelligence}";
        statsTexts[3].text = $"{playerStats.Health}";
        statsTexts[4].text = $"{playerStats.criticalPer * 100}%";
        statsTexts[5].text = $"{playerStats.PhysicalAttack}";
        statsTexts[6].text = $"{playerStats.PhysicalDefense}";
        statsTexts[7].text = $"{playerStats.SkillAttack}";
        statsTexts[8].text = $"{playerStats.SkillDefense}";
        statsTexts[9].text = $"{playerStats.statsPoints}";
        statsTexts[10].text = $"{playerStats.IncreaseMoveSpeedPer * 100}%";
        statsTexts[11].text = $"{playerStats.InCreaseAttackSpeedPer * 100}%";
        statsTexts[12].text = $"{playerStats.CoolDown * 100}%";
        statsTexts[13].text = $"{playerStats.CastingSpeed * 100}%";
        statsTexts[14].text = $"{playerStats.Resistance * 100}%";
        statsTexts[15].text = $"{playerStats.CriticalDamage * 100}%";
    }
    
    /// <summary>
    /// 스탯 포인트에 따른 버튼 활성화
    /// </summary>
    /// <param name="statsPoint">현재 보유중인 스탯포인트가 얼마인지</param>
    private void ActiveIncreaseButton(int statsPoint)
    {  
        for (int i = 0; i < increaseButtons.Count; i++)
        {
            increaseButtons[i].gameObject.SetActive(statsPoint != 0);
        } 
    }
    
}
