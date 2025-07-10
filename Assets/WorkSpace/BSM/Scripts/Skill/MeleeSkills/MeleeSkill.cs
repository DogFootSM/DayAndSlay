using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeSkill : SkillFactory
{ 
    public MeleeSkill(SkillNode skillNode) :base(skillNode){}
    
    /// <summary>
    /// 근접 공격 이펙트
    /// </summary>
    protected void MeleeEffect(Vector2 position, Vector2 direction, string skillId, GameObject skillEffectPrefab)
    {
        GameObject instance = particlePooling.GetSkillPool(skillId, skillEffectPrefab);
        instance.transform.parent = null;
        instance.transform.position = position + direction;
        
        ParticleSystem particleSystem = instance.GetComponent<ParticleSystem>();
        ParticleStopAction stopAction = instance.GetComponent<ParticleStopAction>();
        stopAction.SkillID = skillId;
        
        instance.SetActive(true);
        particleSystem.Play(); 
    } 
}
