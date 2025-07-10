using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillNode
{
    public SkillData skillData;  
    private List<SkillNode> prerequisiteSkillNode = new();
    private List<SkillNode> nextSkillNode = new();
    private PlayerModel playerModel;
    
    private int curSkillLevel = 0;
    private bool unLocked; 
    public bool UnLocked => unLocked;
    public int CurSkillLevel => curSkillLevel;
    
    public SkillNode(SkillData skillData, PlayerModel playerModel)
    {
        this.skillData = skillData;
        this.playerModel = playerModel;
    }
 
    /// <summary>
    /// 선행 조건으로 찍혀있어야 할 스킬 리스트
    /// </summary>
    /// <param name="skillNode">선행 조건의 스킬 노드</param>
    public void AddPrerequisiteSkillNode(SkillNode skillNode)
    {
        prerequisiteSkillNode.Add(skillNode); 
    }

    /// <summary>
    /// 스킬 강화
    /// </summary> 
    public void ApplyPoint()
    {
        //TODO: 스킬 강화 로직 진행
        curSkillLevel++;
        playerModel.CurSkillPoint = -1;

        if (!skillData.IsActive)
        {
            //TODO: 패시브 스킬 능력치 적용 필요
        }
    }

    /// <summary>
    /// 선행 스킬 확인 후 잠금 해제
    /// </summary>
    public void TryUnlockByPrerequisites()
    { 
        foreach (SkillNode preNode in prerequisiteSkillNode)
        {
            unLocked = preNode.curSkillLevel > 0;
        }  
    }

    /// <summary>
    /// 현재 스킬의 레벨 및 해금 정보 불러오기
    /// </summary>
    /// <param name="skillLevel">저장된 스킬 레벨</param>
    /// <param name="lockState">저장된 해금 상태</param>
    public void LoadSkillFromDB(int skillLevel, bool lockState)
    {
        curSkillLevel = skillLevel;
        unLocked = lockState;
    }
    
    
}
