using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveSkill : SkillFactory
{ 
    public PassiveSkill(SkillNode skillNode) : base(skillNode)
    {
    }

    protected void PassiveEffect()
    {
        //TODO: 패시브 이펙트 적용 안할거면 메서드 제거하기
    }
    
}
