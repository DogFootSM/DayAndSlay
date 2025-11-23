using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalusMethodRE : BossMethodRE
{
    public override void Skill_First()
    {
        Debug.Log("뿌리 공격");
        //Todo : 뿌리 공격
        //root.transform.position = player.transform.position;
        //root.Play();
        RootAttack();
    }

    public override void Skill_Second()
    {
        //Todo : 잡몹 소환
        Debug.Log("몬스터 소환함");
        
        //소환후 정상적인 움직임 시키려면 젠젝트 이용하여 돌려야할 것으로 보임
        //테스트씬에서는 테스트하기 껄끄러운 상황
        //DiContainer 이용해서 해야함
        
        //Instantiate(monster, transform.position + new Vector3(-1,0,0), Quaternion.identity);
    }

    public override void Skill_Third()
    {
        Frenzy();
    }

    private void RootAttack()
    {
        skills.SetAllEffectPos(firstSkillData, player.transform.position);
    }

    private void Frenzy()
    {
        if(bossAi == null)
            bossAi = GetComponent<BossAIRe>();
        
        bossAi.skillFirstTimer /= 2;
    }
    
    /// <summary>
    /// Bellus에선 사용하지 않음
    /// </summary>
    public override void Skill_Fourth()
    {}
}
