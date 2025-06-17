using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempOneSkill : MeleeSkill
{
    public TempOneSkill(SkillNode skillNode) : base(skillNode){}

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        MeleeEffect();
        
        Debug.Log(direction);
        Debug.Log($"{skillNode.skillData.SkillId}근접 스킬 사용");
    }
     
}
