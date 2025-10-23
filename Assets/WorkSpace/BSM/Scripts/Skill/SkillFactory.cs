using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillFactory
{
    protected SkillNode skillNode;
    protected LayerMask monsterLayer;
    protected Vector2 overlapSize;
    protected SkillParticlePooling particlePooling => SkillParticlePooling.Instance;
    
    protected int leftHash;
    protected int rightHash;
    protected int upHash;
    protected int downHash;
    
    public SkillFactory(SkillNode skillNode)
    {
        this.skillNode = skillNode;
        this.monsterLayer = LayerMask.GetMask("Monster");
    }

    /// <summary>
    /// 스킬 사용
    /// 각 스킬 기능 동작
    /// </summary>
    /// <param name="direction">캐릭터가 바라보고 있는 방향</param>
    /// <param name="playerPosition">캐릭터가 스킬을 사용한 위치</param>
    public abstract void UseSkill(Vector2 direction, Vector2 playerPosition);

    /// <summary>
    /// 패시브 스킬 능력치 적용
    /// 장착중인 무기에 따라 패시브 능력치 적용
    /// </summary>
    /// <param name="weaponType">장착중인 무기 타입</param>
    public abstract void ApplyPassiveEffects(CharacterWeaponType weaponType);

    /// <summary>
    /// 해당 스킬이 재생할 애니메이션 해시 값을 전달
    /// 바라보고 있는 방향 구분
    /// </summary>
    /// <param name="direction">현재 캐릭터가 바라보고 있는 방향</param>
    /// <returns></returns>
    public int SendSkillAnimationHash(Vector2 direction)
    {
        if (direction == Vector2.right) return rightHash;
        if (direction == -Vector2.right) return leftHash;
        if (direction == Vector2.up) return upHash;
        if (direction == -Vector2.up) return downHash;

        return 0; 
    }
    
    //TODO: 기즈모 테스트용
    public abstract void Gizmos();
}
