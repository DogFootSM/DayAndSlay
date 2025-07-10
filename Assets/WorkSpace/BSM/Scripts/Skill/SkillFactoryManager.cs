using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillFactoryManager 
{
    public static SkillFactory GetSkillFactory(SkillNode skillNode)
    {
        switch (skillNode.skillData.SkillId)
        {
            case "1_검" : return new TempOneSkill(skillNode);
            case "1_창" : return new TempOneSkill(skillNode);
            case "2_창" : return new TempOneSkill(skillNode);
            
            default: return null;
        }    
    } 
}
