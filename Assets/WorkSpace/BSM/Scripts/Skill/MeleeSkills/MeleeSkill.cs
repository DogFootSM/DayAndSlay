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
        GameObject instance = particlePooling.GetSkillPool(position, direction, skillId, skillEffectPrefab);
        Debug.Log($"{skillId} 근접 스킬 이펙트 발동");
        
        ParticleSystem particleSystem = instance.GetComponent<ParticleSystem>();
        
        instance.SetActive(true);
        particleSystem.Play(); 
    } 
}
