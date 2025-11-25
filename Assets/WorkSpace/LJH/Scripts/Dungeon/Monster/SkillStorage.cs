using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class SkillStorage : MonoBehaviour
{
    [SerializeField] [SerializedDictionary]
    public SerializedDictionary<string, MonsterSkillData> skillDataDict;
    
    [SerializeField] [SerializedDictionary]
    public SerializedDictionary<MonsterSkillData, List<GameObject>> skillVFXDict;

    public List<Effect> GetSkillVFX(MonsterSkillData skillData)
    {
        List<Effect> result = new List<Effect>();

        for (int i = 0; i < skillVFXDict[skillData][0].transform.childCount; i++)
        {
            result.Add(skillVFXDict[skillData][0].transform.GetChild(i).GetComponent<Effect>());
        }
        
        return result;
    }

    public List<SpriteRenderer> GetSkillWarning(MonsterSkillData skillData)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();

        for (int i = 0; i < skillVFXDict[skillData][1].transform.childCount; i++)
        {
            result.Add(skillVFXDict[skillData][1].transform.GetChild(i).GetComponent<SpriteRenderer>());
        }
        
        return result;
    }

    public List<Collider2D> GetSkillRadius(MonsterSkillData skillData)
    {
        List<Collider2D> result = new List<Collider2D>();

        for (int i = 0; i < skillVFXDict[skillData][2].transform.childCount; i++)
        {
            result.Add(skillVFXDict[skillData][2].transform.GetChild(i).GetComponent<Collider2D>());
        }
        
        return result;
    }

    public void SetAllEffectPos(MonsterSkillData skill, Vector3 targetPos)
    {
            // 각 그룹별 transform 부모
            Transform effectGroup = skillVFXDict[skill][0].transform;
            Transform warningGroup = skillVFXDict[skill][1].transform;
            Transform radiusGroup = skillVFXDict[skill][2].transform;

            int count = effectGroup.childCount;

            for (int i = 0; i < count; i++)
            {
                effectGroup.GetChild(i).position = targetPos;
                warningGroup.GetChild(i).position = targetPos;
                radiusGroup.GetChild(i).position = targetPos;
            }
    }
    
    public void SetSkillEffectRandomSpread(MonsterSkillData skill, float range)
    {
        Transform effectGroup  = skillVFXDict[skill][0].transform;
        Transform warningGroup = skillVFXDict[skill][1].transform;
        Transform radiusGroup  = skillVFXDict[skill][2].transform;

        int count = effectGroup.childCount;

        for (int i = 0; i < count; i++)
        {
            // 자식별 랜덤 위치 생성
            Vector3 randomPos = new Vector3(
                Random.Range(transform.position.x - range, transform.position.x + range),
                Random.Range(transform.position.y - range, transform.position.y + range),
                0f
            );

            // 같은 인덱스끼리는 같은 위치로 이동
            effectGroup.GetChild(i).position = randomPos;
            warningGroup.GetChild(i).position = randomPos;
            radiusGroup.GetChild(i).position = randomPos;
        }
    }
    
    
    
}
