using System.Collections;
using UnityEngine;

public class BansheeMethod : BossMonsterMethod
{
    public override void Skill_First()
    {
        throw new System.NotImplementedException();
    }

    public override void Skill_Second()
    {
        throw new System.NotImplementedException();
    }

    public override void Skill_Third()
    {
        throw new System.NotImplementedException();
    }

    public override void Skill_Fourth()
    {
        throw new System.NotImplementedException();
    }
}
/*{
    [SerializeField] SpiritRush spirit;
    
    [SerializeField] private float buffDuration;
    
    public override void Skill_First()
    {
        Debug.Log("밴시가 비명을 지릅니다.");
    }

    public override void Skill_Second()
    {
        Debug.Log("밴시가 이동합니다.");
    }

    public override void Skill_Third()
    {
        Debug.Log("버프 사용");
        StartCoroutine(MoveSpeedBuff());
    }

    public override void Skill_Fourth()
    {
        Debug.Log("궁극기 사용");
        spirit.SetRushDir(animator.GetCurrentDir());
        spirit.GetComponent<ParticleSystem>().Play();
        spirit.Rush();
    }

    /// <summary>
    /// 애니메이션 이벤트로 호출
    /// </summary>
    public void Skill_FourthEffectEnd()
    {
        spirit.EndRush();
    }

    /// <summary>
    /// 이동속도 증가 버프
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveSpeedBuff()
    {
        float preMoveSpeed = monsterData.MoveSpeed;
        monsterData.MoveSpeed *= 1.5f;
        yield return new WaitForSeconds(buffDuration);
        monsterData.MoveSpeed = preMoveSpeed;
    }


    /// <summary>
    /// 텔레포트 스킬 / 애니메이션 이벤트로 사용
    /// </summary>
    public void Teleport_Banshee()
    {
        Vector3 pos = player.transform.position;
        
        float randomX = Random.Range(pos.x - 3, pos.x + 3);
        float randomy = Random.Range(pos.y - 3, pos.y + 3);
        
        
        Vector3 randomPos = new Vector3(randomX, randomy, 0);
        
        transform.position = randomPos;
    }

}*/
