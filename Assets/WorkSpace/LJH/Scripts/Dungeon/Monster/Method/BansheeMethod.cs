using UnityEngine;

public class BansheeMethod : BossMonsterMethod
{
    private BossMonsterAI benshee;
    
    [SerializeField] private GameObject stompEffect;

    public override void Skill_First()
    {
        Debug.Log("비명");
    }

    public override void Skill_Second()
    {
        //밴시 사라지면서
        //랜덤한 구역으로 이동
        //다시 나타남
        //이후 스크림 사용
        Debug.Log("텔레포트");
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
