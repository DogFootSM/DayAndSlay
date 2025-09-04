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
        
    }
    
    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        
        LightManager.Instance.LightningFlash();
        GetMonstersCenter(playerPosition);
        //TODO: Hit Action 추가 필요
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
        skillNode.PlayerSkillReceiver.StartCoroutine(WaitForDarknessRoutine(center, cols.Length));
    }

    private IEnumerator WaitForDarknessRoutine(Vector2 center, int length)
    {
        //번개 화면 연출이 끝날 때까지 대기
        yield return new WaitUntil(() => LightManager.Instance.IsLightningFlash);
        SkillEffect(center, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);

        //감지된 몬스터가 있을 경우에만 창 이펙트 재생
        if (length > 0)
        {
            SpawnParticleAtRandomPosition(center, skillNode.skillData.SkillRadiusRange, 2f, skillNode.skillData.SkillEffectPrefab[1], $"{skillNode.skillData.SkillId}_2_Particle", 50);
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
