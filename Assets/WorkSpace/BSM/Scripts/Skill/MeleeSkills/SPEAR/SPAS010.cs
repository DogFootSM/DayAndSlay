using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SPAS010 : MeleeSkill
{
    private Vector2 pos;
    
    public SPAS010(SkillNode skillNode) : base(skillNode)
    {
        leftHash = Animator.StringToHash("SkillMotion_Left_10");
        rightHash = Animator.StringToHash("SkillMotion_Right_10");
        upHash = Animator.StringToHash("SkillMotion_Up_10");
        downHash = Animator.StringToHash("SkillMotion_Down_10");
    }
    
    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        GetMonstersCenter(playerPosition);
        
        pos = playerPosition; 
    }

    /// <summary>
    /// 캐릭터의 위치에서 감지한 몬스터들의 중심 위치를 구함
    /// </summary>
    /// <param name="playerPosition">현재 캐릭터가 서있는 위치</param>
    private void GetMonstersCenter(Vector2 playerPosition)
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition, overlapSize, 0, monsterLayer);

        float cx = 0;
        float cy = 0;

        for (int i = 0; i < cols.Length; i++)
        {
            cx += cols[i].transform.position.x;
            cy += cols[i].transform.position.y;
        }

        cx /= cols.Length;
        cy /= cols.Length;
        
        //감지된 몬스터가 없을 경우 플레이어 위치로 설정
        if (cols.Length == 0)
        {
            cx = playerPosition.x;
            cy = playerPosition.y;
        }
        
        Vector2 center = new Vector2(cx, cy);
        
        SoundManager.Instance.PlaySfx(SFXSound.SPAS010);
        SkillEffect(center, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);

        skillDamage = GetSkillDamage();
        
        //감지된 몬스터가 있을 경우에만 창 이펙트 재생
        if (cols.Length > 0)
        {
            SpawnParticleAtRandomPosition(center, skillNode.skillData.SkillRadiusRange, 1.5f, skillNode.skillData.SkillEffectPrefab[1], $"{skillNode.skillData.SkillId}_2_Particle", 50);
            DelayHit(cols);
        }
         
    }

    private void DelayHit(Collider2D[] cols)
    {
        skillNode.PlayerSkillReceiver.StartCoroutine(DelayHitCoroutine(cols));
    }

    private IEnumerator DelayHitCoroutine(Collider2D[] cols)
    {
        yield return WaitCache.GetWait(1.5f);
        
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i] == null) continue;
            
            IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
            Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount);
        } 
    }
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
        UnityEngine.Gizmos.color = Color.blue;

        UnityEngine.Gizmos.DrawWireCube(pos, overlapSize);
    }
}
