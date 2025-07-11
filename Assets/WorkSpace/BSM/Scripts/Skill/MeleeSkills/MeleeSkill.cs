using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeSkill : SkillFactory
{

    private ParticleSystem.MainModule mainModule;
    private Vector2 currentDirection;
    
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
        
        mainModule = particleSystem.main;
        currentDirection = direction;
        
        instance.SetActive(true);
        particleSystem.Play(); 
    }

    /// <summary>
    /// 파티클의 StartRotation을 회전
    /// </summary>
    /// <param name="leftDeg">왼쪽 방향일 경우의 회전 값</param>
    /// <param name="rightDeg">오른쪽 방향일 경우의 회전 값 </param>
    /// <param name="downDegY">아래 방향일 경우의 회전 값</param>
    /// <param name="upDegY">윗 방향일 경우의 회전 값</param>
    protected void SetParticleStartRotationFromDeg(float leftDeg, float rightDeg, float downDegY, float upDegY)
    { 
        if (currentDirection.x < 0) mainModule.startRotationZ = Mathf.Deg2Rad * rightDeg;
        if(currentDirection.x > 0) mainModule.startRotationZ = Mathf.Deg2Rad * leftDeg;
        if (currentDirection.y < 0) mainModule.startRotationZ = Mathf.Deg2Rad * upDegY;
        if (currentDirection.y > 0) mainModule.startRotationZ = Mathf.Deg2Rad * downDegY;
    }
    
}
