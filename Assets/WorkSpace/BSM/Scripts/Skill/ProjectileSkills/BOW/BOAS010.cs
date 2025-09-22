using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BOAS010 : MeleeSkill
{
    public BOAS010(SkillNode skillNode) : base(skillNode)
    {
    }

    private Vector2 hitPos;
    private float offset = 4f;
    private float minRot = 235f;
    private float maxRot = 255f;
    private float verticalDistance = 2f;
    
    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        skillDamage = GetSkillDamage();
        
        hitPos = playerPosition + (direction * (skillNode.skillData.SkillRadiusRange / 2));
        Collider2D[] cols = Physics2D.OverlapBoxAll(hitPos, overlapSize, 0, monsterLayer);
        
        skillNode.PlayerSkillReceiver.StartCoroutine(SkillEffectRoutine(playerPosition, direction, cols)); 
    }

    private IEnumerator SkillEffectRoutine(Vector2 playerPosition, Vector2 direction, Collider2D[] cols)
    {
        float elapsedTime = 0;
        
        float minX = -overlapSize.x / 2;
        float maxX = overlapSize.x / 2;
        float maxY = overlapSize.y / 2;
        int index = 0;
        
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            
            float x = Random.Range(minX, maxX);
            float y = Random.Range(0, maxY);
            
            SkillEffect(hitPos + new Vector2(x, y), index, $"{skillNode.skillData.SkillId}_1_Particle",skillNode.skillData.SkillEffectPrefab[0]);
            
            instance.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(minRot, maxRot));
            
            interactions[index++].LinearProjectile(0, instance.transform.right, verticalDistance);
            yield return WaitCache.GetWait(0.01f);
        }
        
        interactions[0].SetHitEffectId($"{skillNode.skillData.SkillId}_3_Particle");
        
        for (int i = 0; i < cols.Length; i++)
        {
            IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
            Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount);
            interactions[0].PlayHitEffect(cols[i].transform.position, Vector2.zero);
        }
        
    }
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
        UnityEngine.Gizmos.color = Color.yellow;
        UnityEngine.Gizmos.DrawWireCube(hitPos, overlapSize);
    }
}