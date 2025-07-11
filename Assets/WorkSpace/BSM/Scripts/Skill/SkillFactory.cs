using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillFactory
{
    protected SkillNode skillNode;
    protected LayerMask monsterLayer;
    protected Vector2 overlapSize;
    protected SkillParticlePooling particlePooling => SkillParticlePooling.Instance;
    
    public SkillFactory(SkillNode skillNode)
    {
        this.skillNode = skillNode;
        this.monsterLayer = LayerMask.GetMask("Monster");
    }

    public abstract void UseSkill(Vector2 direction, Vector2 playerPosition);
    
    //TODO: 기즈모 테스트용
    public abstract void Gizmos();
}
