using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalusMethod : BossMethod
{
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private BellusMethod bellus;
    private MalusAI malusAi;

    
    public override void Skill_First()
    {
        Debug.Log("뿌리 공격");
        
        RootAttack();
    }

    public override void Skill_Second()
    {
        Debug.Log("몬스터 소환함");
        
        SummonMonster();
    }

    public override void Skill_Third()
    {
        Frenzy();
    }

    private void RootAttack()
    {
        skills.SetAllEffectPos(firstSkillData, player.transform.position);
    }

    private void SummonMonster()
    {
        GameObject[] summonMonsterList = new GameObject[] {};
        
        summonMonsterList = GameObject.FindGameObjectsWithTag("Monster");

        if (summonMonsterList.Length >= 2) return;
        
        monsterPrefab.GetComponentInChildren<TargetSensor>().SetGrid(GameObject.Find("BossRoom").GetComponent<Grid>());
        Instantiate(monsterPrefab, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
    }

    private void Frenzy()
    {
        if(malusAi == null)
            malusAi = GetComponent<MalusAI>();
        
        if(bossAi == null)
            bossAi = GetComponent<BossAI>();
        
        malusAi.SetIsFrenzy(true);
        bossAi.skillFirstTimer /= 2;
    }
    
    /// <summary>
    /// Bellus에선 사용하지 않음
    /// </summary>
    public override void Skill_Fourth()
    {}
    
}
