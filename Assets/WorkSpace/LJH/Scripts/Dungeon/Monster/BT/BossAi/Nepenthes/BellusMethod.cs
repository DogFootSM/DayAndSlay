using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellusMethod : BossMethod
{
    private BossAI bellus;
    [SerializeField] private BossAI malus;
    
    protected override void Start()
    {
        base.Start();
        bellus = GetComponent<BellusAI>();
        
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
        SeedBomb();
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

    private void SeedBomb()
    {
        skills.SetSkillEffectRandomSpread(thirdSkillData, 4.5f);
    }

    private IEnumerator SeedActiveCoroutine()
    {
        List<Effect> seedEffects = skills.GetSkillVFX(thirdSkillData);
        foreach (var seed in seedEffects)
        {
            Vector3 seedPos = seed.transform.position;
            
            seed.gameObject.transform.position = new Vector3(Random.Range(seedPos.x - 3, seedPos.x + 3), 
                Random.Range(seedPos.y - 3, seedPos.y + 3),0);
            
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
