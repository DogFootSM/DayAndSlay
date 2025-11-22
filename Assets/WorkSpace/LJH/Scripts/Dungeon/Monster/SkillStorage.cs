using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class SkillStorage : MonoBehaviour
{
    public static SkillStorage instance;
    
    
    [SerializeField] [SerializedDictionary]
    private SerializedDictionary<string, MonsterSkillData> skillDataDict;
    
    [SerializeField] [SerializedDictionary]
    private SerializedDictionary<MonsterSkillData, GameObject> skillVFXDict;

    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public Effect GetSkillVFX(MonsterSkillData skillData) => skillVFXDict[skillData].transform.GetChild(0).GetComponent<Effect>();
}
