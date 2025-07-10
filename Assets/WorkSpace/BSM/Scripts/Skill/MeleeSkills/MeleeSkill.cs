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
        
        ParticleSystem.MainModule main = particleSystem.main;
         
        //좌 -1 90 우 1, 270 아래 -1 180 위 1 0
 
        if (direction.x < 0) main.startRotationZ = 270;
        if(direction.x > 0) main.startRotationZ = 90;
        if (direction.y < 0) main.startRotationZ = 0;
        if (direction.y > 0) main.startRotationZ = 180;

        instance.SetActive(true);
        particleSystem.Play(); 
    } 
}
