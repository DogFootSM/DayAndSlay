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
            case "SSAS004" : return new SSAS004(skillNode);
            case "SSAS005" : return new SSAS005(skillNode);
            case "SSAS006" : return new SSAS006(skillNode);
            case "SSAS007" : return new SSAS007(skillNode);
            case "SSAS008" : return new SSAS008(skillNode);
            case "SSAS009" : return new SSAS009(skillNode);
            case "SSAS010" : return new SSAS010(skillNode); 
            case "SSPS001" : return new SSPS001(skillNode);
            case "SSPS002" : return new SSPS002(skillNode);
            case "SSPS003" : return new SSPS003(skillNode);
            case "SSPS004" : return new SSPS004(skillNode);
            case "SPAS001" : return new SPAS001(skillNode);
            case "SPAS008" : return new SPAS008(skillNode);
            case "2_창" : return new SSAS002(skillNode);
            case "3_창" : return new SSAS003(skillNode);
            case "4_창" : return new SSPS001(skillNode);
            
            default: return null;
        }    
    } 
}
