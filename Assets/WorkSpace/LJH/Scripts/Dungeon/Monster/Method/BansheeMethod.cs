using System.Collections;
using UnityEngine;

public class BansheeMethod : BossMonsterMethod
{
    private BossMonsterAI benshee;
    
    public override void Skill_First()
    {
    }

    public override void Skill_Second()
    {
        
    }


    public void Teleport_Banshee()
    {
        Debug.Log("텔레포트 사용");
        Vector3 pos = player.transform.position;
        
        float randomX = Random.Range(pos.x - 3, pos.x + 3);
        float randomy = Random.Range(pos.y - 3, pos.y + 3);
        
        
        Vector3 randomPos = new Vector3(randomX, randomy, 0);
        
        transform.position = randomPos;
    }

}
