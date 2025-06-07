using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillNode
{
    public SkillData skillData;  
    private List<SkillNode> prerequisiteSkillNode = new();
    private List<SkillNode> nextSkillNode = new();

    private bool unLocked;

    public SkillNode(SkillData skillData)
    {
        this.skillData = skillData;
    }
     
    /// <summary>
    /// 선행 조건으로 찍혀있어야 할 스킬 리스트
    /// </summary>
    /// <param name="skillNode">선행 조건의 스킬 노드</param>
    public void AddPrerequisiteSkillNode(SkillNode skillNode)
    {
        prerequisiteSkillNode.Add(skillNode);
    }
 
}
