using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalusMethod : BossMonsterMethod
{
    private BossMonsterAI malus;
    private BossMonsterAI bellus;

    private void Start()
    {
        bellus = GetComponent<Bellus>();
        malus = ((Bellus)bellus).GetPartner();
    }
    public override void Skill_First()
    {
        //Èú ½ºÅ³
        bellus.GetMonsterModel().SetMonsterHp(10);
        malus.GetMonsterModel().SetMonsterHp(10);
    }

    public override void Skill_Second()
    {
        
    }
}