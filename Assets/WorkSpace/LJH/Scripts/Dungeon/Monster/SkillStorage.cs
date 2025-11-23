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
        skillVFXDict[skill][2].transform.position = targetPos;
        skillVFXDict[skill][1].transform.position = targetPos;
        skillVFXDict[skill][0].transform.position = targetPos;
    }
}
