using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAi : GeneralMonsterAI
{
    public override void Attack()
    {
        Debug.Log("고블린 공격");
        animator?.PlayAttack();
    }

    public override void Move()
    {
        Debug.Log("고블린 이동");
        animator?.PlayMove();
    }
}
