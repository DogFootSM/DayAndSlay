using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellusMethod : BossMonsterMethod
{
    private BossMonsterAI bellus;
    private BossMonsterAI malus;

    private void Start()
    {
        bellus = GetComponent<Bellus>();
        malus = ((Bellus)bellus).GetPartner();
    }
    public override void Skill()
    {
        //Èú ½ºÅ³
        bellus.GetMonsterModel().SetMonsterHp(10);
        malus.GetMonsterModel().SetMonsterHp(10);
    }
}
