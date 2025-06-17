using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillFactory
{
    protected SkillNode skillNode;

    public SkillFactory(SkillNode skillNode)
    {
        this.skillNode = skillNode;
    }

    public abstract void UseSkill(Vector2 direction, Vector2 playerPosition);
}
