using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillFactoryManager 
{
    public static SkillFactory GetSkillFactory(SkillNode skillNode)
    {
        switch (skillNode.skillData.SkillId)
        {
            //검
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
            
            //창
            case "SPAS001" : return new SPAS001(skillNode);
            case "SPAS002" : return new SPAS002(skillNode);
            case "SPAS003" : return new SPAS003(skillNode);
            case "SPAS004" : return new SPAS004(skillNode);
            case "SPAS005" : return new SPAS005(skillNode);
            case "SPAS006" : return new SPAS006(skillNode);
            case "SPAS007" : return new SPAS007(skillNode);
            case "SPAS008" : return new SPAS008(skillNode);
            case "SPAS009" : return new SPAS009(skillNode);
            case "SPAS010" : return new SPAS010(skillNode); 
            case "SPPS001" : return new SPPS001(skillNode);
            case "SPPS002" : return new SPPS002(skillNode);
            case "SPPS003" : return new SPPS003(skillNode);
            case "SPPS004" : return new SPPS004(skillNode);
            
            //활
            case "BOAS001" : return new BOAS001(skillNode);
            case "BOAS002" : return new BOAS002(skillNode);
            case "BOAS003" : return new BOAS003(skillNode);
            case "BOAS004" : return new BOAS004(skillNode);
            case "BOAS005" : return new BOAS005(skillNode);
            case "BOAS006" : return new BOAS006(skillNode);
            case "BOAS007" : return new BOAS007(skillNode);
            case "BOAS008" : return new BOAS008(skillNode);
            case "BOAS009" : return new BOAS009(skillNode);
            
            case "BOPS001" : return new BOPS001(skillNode);
            case "BOPS002" : return new BOPS002(skillNode);
            case "BOPS003" : return new BOPS003(skillNode);
            case "BOPS004" : return new BOPS004(skillNode);
            default: return null;
        }    
    } 
}
