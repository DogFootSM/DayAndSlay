using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellusMethod : BossMonsterMethod
{
    private BossMonsterAI bellus;
    private BossMonsterAI malus;
    
    [SerializeField] private GameObject bellusHealEffect;
    [SerializeField] private GameObject malusHealEffect;
    [SerializeField] private GameObject poisonWarnigEffect;
    [SerializeField] private GameObject poisonEffect;

    private void Start()
    {
        bellus = GetComponent<Bellus>();
        malus = ((Bellus)bellus).GetPartner();
    }
    public override void Skill_First()
    {
        //힐 스킬
        SetEffectActiver(bellusHealEffect);
        SetEffectActiver(malusHealEffect);
        
        bellus.GetMonsterModel().SetMonsterHp(10);
        malus.GetMonsterModel().SetMonsterHp(10);
    }

    public override void Skill_Second()
    {
        //독 장판 설치
        SetPosEffect(poisonWarnigEffect);
        SetPosEffect(poisonEffect);
        
        SetEffectActiver(poisonWarnigEffect);
        SetEffectActiver(poisonEffect);
    }
    
}

