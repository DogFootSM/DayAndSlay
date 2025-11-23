using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellusMethodRE : BossMethodRE
{
    private BossAIRe bellus;
    [SerializeField] private BossMonsterAI malus;


    [SerializeField] private List<ParticleSystem> seedEffects;
    protected override void Start()
    {
        base.Start();
        bellus = GetComponent<BellusAIRE>();
        
    }
    public override void Skill_First()
    {
        Debug.Log("독장판 실행");
        Poison();

    }

    public override void Skill_Second()
    {
        Debug.Log("힐 실행");

        Heal();
    }

    public override void Skill_Third()
    {
        Debug.Log("씨앗뿌리기");
        StartCoroutine(SeedActiveCoroutine());
    }

    private void Poison()
    {
        skills.SetAllEffectPos(firstSkillData, player.transform.position);
    }

    private void Heal()
    {
        float b_curHp = bellus.GetMonsterModel().CurHp;
        float b_maxHp = bellus.GetMonsterModel().MaxHp;
        
        
        if (b_curHp < b_maxHp)
        {
            bellus.GetMonsterModel().SetMonsterHp(25);
            malus.GetMonsterModel().SetMonsterHp(25);
            
        }
    }

    private IEnumerator SeedActiveCoroutine()
    {
        List<Effect> seedEffects = skills.GetSkillVFX(thirdSkillData);
        foreach (var seed in seedEffects)
        {
            Vector3 seedPos = seed.transform.position;
            
            seed.gameObject.transform.position = new Vector3(Random.Range(seedPos.x - 6, seedPos.x + 6), 
                Random.Range(seedPos.y - 6, seedPos.y + 6),0);
            
            seed.PlaySkill();
            yield return new WaitForSeconds(0.4f);
        }
    }
    
    /// <summary>
    /// Bellus에선 사용하지 않음
    /// </summary>
    public override void Skill_Fourth()
    {}

}
