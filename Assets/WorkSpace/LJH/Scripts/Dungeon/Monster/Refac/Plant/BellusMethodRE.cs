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
        firstSkillData.SetSkillRadius(player.transform.position);
    }

    private void Heal()
    {
        if (bellus.GetMonsterModel().CurHp >= bellus.GetMonsterModel().MaxHp &&
            bellus.GetMonsterModel().CurHp >= bellus.GetMonsterModel().MaxHp)
        {
            bellus.GetMonsterModel().SetMonsterHp(25);
            malus.GetMonsterModel().SetMonsterHp(25);
        }
    }

    private IEnumerator SeedActiveCoroutine()
    {
        foreach (ParticleSystem seed in seedEffects)
        {
            Vector3 seedPos = seed.transform.position;
            
            seed.gameObject.transform.position = new Vector3(Random.Range(seedPos.x - 6, seedPos.x + 6), 
                Random.Range(seedPos.y - 6, seedPos.y + 6),0);
            
            seed.Play();
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    /// <summary>
    /// Bellus에선 사용하지 않음
    /// </summary>
    public override void Skill_Fourth()
    {}

}
