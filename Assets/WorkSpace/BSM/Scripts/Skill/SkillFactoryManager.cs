using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillFactoryManager 
{
    public static SkillFactory GetSkillFactory(SkillNode skillNode)
    {
        switch (skillNode.skillData.SkillId)
        {
            //TODO: 스킬 아이디 값 수정 필요
            case "SSAS001" : return new SSAS001(skillNode);
            case "SSAS002" : return new SSAS002(skillNode);
            case "SSAS003" : return new SSAS003(skillNode);
            case "SSAS004" : return new SSAS001(skillNode);
            case "SSAS005" : return new SSAS001(skillNode);
            case "SSAS006" : return new SSAS001(skillNode);
            case "SSAS007" : return new SSAS001(skillNode);
            case "SSAS008" : return new SSAS001(skillNode);
            case "SSAS009" : return new SSAS001(skillNode);
            case "SSAS010" : return new SSAS001(skillNode); 
            case "1_창" : return new SSAS001(skillNode);
            case "2_창" : return new SSAS002(skillNode);
            case "3_창" : return new SSAS003(skillNode);
            case "4_창" : return new SSPS001(skillNode);
            
            default: return null;
        }    
    } 
}
