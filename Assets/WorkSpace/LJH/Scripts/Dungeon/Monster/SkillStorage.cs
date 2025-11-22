using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class SkillStorage : MonoBehaviour
{
    public static SkillStorage instance;
    
    
    [SerializeField] [SerializedDictionary]
    private SerializedDictionary<string, MonsterSkillData> skillDataDict;
    
    [SerializeField] [SerializedDictionary]
    private SerializedDictionary<MonsterSkillData, List<GameObject>> skillVFXDict;

    
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

    private void Start()
    {
        SetSkillDataVFX();
    }

    public void SetSkillDataVFX()
    {
        foreach (var skillData in skillVFXDict.Keys)
        {
            skillData.SetVfx(skillVFXDict[skillData]);
        }
    }

    public Effect GetSkillVFX(MonsterSkillData skillData) => skillData.SkillEffect.GetComponent<Effect>();
    public SpriteRenderer GetSkillWarning(MonsterSkillData skillData) => skillData.WarningEffect;
    public Collider2D GetSkillRadius(MonsterSkillData skillData) => skillData.AttackCollider;
}
