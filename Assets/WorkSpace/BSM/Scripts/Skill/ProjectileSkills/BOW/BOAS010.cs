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

    private Vector2 pos;
    private Vector2 dir;

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        skillDamage = GetSkillDamage();

        pos = playerPosition;
        dir = direction;

        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition + (direction * 4), overlapSize, 0, monsterLayer);
        
        skillNode.PlayerSkillReceiver.StartCoroutine(SkillEffectRoutine(playerPosition, direction, cols)); 
    }

    private IEnumerator SkillEffectRoutine(Vector2 playerPosition, Vector2 direction, Collider2D[] cols)
    {
        float elapsedTime = 0;
        
        float minX = -overlapSize.x / 2;
        float maxX = overlapSize.x / 2;
        float maxY = overlapSize.y / 2;
        
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            
            float x = Random.Range(minX, maxX);
            float y = Random.Range(0, maxY);
            
            GameObject instance = particlePooling.GetSkillPool($"{skillNode.skillData.SkillId}_1_Particle",
                skillNode.skillData.SkillEffectPrefab[0]);
            
            instance.SetActive(true);
            instance.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(235, 255));
            instance.transform.position = playerPosition + (direction * 4) + new Vector2(x, y);
            
            ParticleSystem particleSystem = instance.GetComponent<ParticleSystem>();
            particleSystem.Play();
            
            ParticleInteraction interaction = instance.GetComponent<ParticleInteraction>();
            interaction.EffectId = $"{skillNode.skillData.SkillId}_1_Particle";
            interaction.LinearProjectile(0, instance.transform.right, 2);
 
            yield return WaitCache.GetWait(0.01f);
        }

        for (int i = 0; i < cols.Length; i++)
        {
            IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
            Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount);
        }
        
    }
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
        UnityEngine.Gizmos.color = Color.yellow;
        UnityEngine.Gizmos.DrawWireCube(pos + (dir * 4), overlapSize);
    }
}