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
    
    public SkillNode(SkillData skillData, PlayerModel playerModel)
    {
        this.skillData = skillData;
        this.playerModel = playerModel;
        //선행 스킬이 없을 경우 잠금 해제
        unLocked = skillData.prerequisiteSkillsId.Count == 0;
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
    
}
