using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileSkill : SkillFactory
{
    private SkillParticlePooling skillParticlePool => SkillParticlePooling.Instance;
    private GameObject surrounfEffectInstance;
    private ParticleSystem surroundParticleSystem;
    private ParticleInteraction surroundInteraction;
    
    protected ParticleSystemRenderer particleSystemRenderer;

    protected float skillDamage;
    
    public ProjectileSkill(SkillNode skillNode) : base(skillNode)
    {
    }

    /// <summary>
    /// 스킬 사용 시 주변에 재생될 이펙트
    /// </summary>
    /// <param name="position">이펙트의 위치</param>
    /// <param name="effectPrefab">재생할 이펙트 오브젝트</param>
    /// <param name="effectId">풀에 반납할 이펙트 아이디</param>
    protected void SurroundEffect(Vector2 position, GameObject effectPrefab, string effectId)
    {
        //파티클 풀에서 파티클 오브젝트 꺼내옴
        surrounfEffectInstance = skillParticlePool.GetSkillPool(effectId, effectPrefab);
        surrounfEffectInstance.transform.position = position;
        surrounfEffectInstance.transform.parent = null;
        surrounfEffectInstance.SetActive(true);
        
        //파티클 재생
        surroundParticleSystem = surrounfEffectInstance.GetComponent<ParticleSystem>();
        surroundParticleSystem.Play();
        
        //풀에 반납할 파티클 아이디 설정
        surroundInteraction = surrounfEffectInstance.GetComponent<ParticleInteraction>();
        surroundInteraction.EffectId = effectId; 
        
        //파티클 렌더러 모드
        particleSystemRenderer = surrounfEffectInstance.GetComponent<ParticleSystemRenderer>();
    }
    
    /// <summary>
    /// 주변에 생성될 이펙트 로테이션 설정
    /// </summary>
    /// <param name="direction">스킬을 사용한 방향</param>
    /// <param name="leftDeg">왼쪽 방향으로 사용 시 설정할 각도</param>
    /// <param name="rightDeg">오른쪽 방향으로 사용 시 설정할 각도</param>
    /// <param name="upDeg">위쪽 방향으로 사용 시 설정할 각도</param>
    /// <param name="downDeg">아래쪽 방향으로 사용 시 설정할 각도</param>
    protected void SetSurroundPrefabLocalRotation(Vector2 direction, float leftDeg, float rightDeg, float upDeg, float downDeg)
    {
        if(direction.x > 0) surrounfEffectInstance.transform.localRotation = Quaternion.Euler(0, 0, rightDeg);
        if(direction.x < 0) surrounfEffectInstance.transform.localRotation = Quaternion.Euler(0, 0, leftDeg);
        if(direction.y > 0) surrounfEffectInstance.transform.localRotation = Quaternion.Euler(0, 0, upDeg);
        if(direction.y < 0) surrounfEffectInstance.transform.localRotation = Quaternion.Euler(0, 0, downDeg); 
    }

    /// <summary>
    /// 해당 스킬의 레벨에 따른 데미지 설정
    /// </summary>
    /// <param name="skillDamage"></param>
    protected void SetSkillDamage(float skillDamage)
    {
        //TODO: 스킬 데미지 공식 수정 필요
        this.skillDamage = skillDamage + (skillDamage * skillNode.CurSkillLevel);
    }
    
    
}
