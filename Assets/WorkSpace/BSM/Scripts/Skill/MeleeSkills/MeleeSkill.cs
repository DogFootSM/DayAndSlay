using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeSkill : SkillFactory
{ 
    public MeleeSkill(SkillNode skillNode) :base(skillNode){}
    
    protected void MeleeEffect()
    {
        Debug.Log("근접 스킬 이펙트 발동");
    }
    
}
