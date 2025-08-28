using UnityEngine;

public class BellusMethod : BossMonsterMethod
{
    private BossMonsterAI bellus;
    private BossMonsterAI malus;
    
    [SerializeField] private ParticleSystem bellusHealEffect;
    [SerializeField] private ParticleSystem malusHealEffect;
    [SerializeField] private ParticleSystem poisonWarnigEffect;
    [SerializeField] private ParticleSystem poisonEffect;
    

    private void Start()
    {
        bellus = GetComponent<Bellus>();
        malus = ((Bellus)bellus).GetPartner();
    }
    public override void Skill_First()
    {
        //힐 스킬
        
        bellus.GetMonsterModel().SetMonsterHp(10);
        malus.GetMonsterModel().SetMonsterHp(10);
    }

    public override void Skill_Second()
    {
        //독 장판 설치
        //SetPosEffect(poisonWarnigEffect, player);
        //SetPosEffect(poisonEffect, player);
        //
        //EffectPlay(poisonWarnigEffect);
        //EffectPlay(poisonEffect);
    }

    public override void Skill_Third()
    {
    }
    public override void Skill_Fourth()
    {
    }

}

