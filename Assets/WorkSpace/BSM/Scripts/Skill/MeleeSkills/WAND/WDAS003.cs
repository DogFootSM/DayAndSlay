using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WDAS003 : MeleeSkill
{
    private Action action;
    private Coroutine castingCo;
    
    public WDAS003(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ExecuteCasting(skillNode.skillData.SkillCastingTime);
        
        ListClear();
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);

        action = () => ExecutePostCastAction(playerPosition);
        castingCo = skillNode.PlayerSkillReceiver.StartCoroutine(WaitCastingRoutine(action)); 
    }

    /// <summary>
    /// 캐스팅 이후 동작
    /// </summary>
    /// <param name="playerPosition"></param>
    private void ExecutePostCastAction(Vector2 playerPosition)
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition, overlapSize, 0, monsterLayer);
        SoundManager.Instance.PlaySfx(SFXSound.WDAS003);
        //레벨당 슬로우 효과
        float slowLevelPer = skillNode.skillData.SkillAbilityValue + ((skillNode.CurSkillLevel - 1) * skillNode.skillData.SkillAbilityFactor);
        
        for (int i = 0; i < cols.Length; i++)
        {
            IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
            ExecuteSlow(receiver, skillNode.skillData.DeBuffDuration,slowLevelPer);
        }
        
        skillNode.PlayerSkillReceiver.StartCoroutine(PostCastRoutine(playerPosition));
    }

    private IEnumerator PostCastRoutine(Vector2 playerPosition)
    {
        float elapsedTime = 0;
        int index = 0;
        
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime * 3f;
            
            float offsetX = Random.Range(-overlapSize.x / 2, overlapSize.x / 2);
            float offsetY = Random.Range(-overlapSize.y / 2, overlapSize.y / 2);
            
            Vector2 randOffset = new Vector2(offsetX, offsetY);
            
            SkillEffect(playerPosition + randOffset, index++, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
            
            yield return WaitCache.GetWait(0.02f);
        } 
    }
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
