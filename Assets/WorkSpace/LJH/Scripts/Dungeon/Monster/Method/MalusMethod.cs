using UnityEngine;

public class MalusMethod : BossMonsterMethod
{
    private BossMonsterAI malus;
    [SerializeField] private BossMonsterAI bellus;

    [SerializeField] private ParticleSystem root;
    [SerializeField] private GameObject monster;

    private void Start()
    {
        base.Start();
        
        malus = GetComponent<MalusAI>();
        //bellus = ((MalusAI)malus).GetPartner();
    }
    public override void Skill_First()
    {
        //Todo : 뿌리 공격
        root.transform.position = player.transform.position;
        root.Play();
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
        ((MalusAI)malus).Frenzy();
    } 
    
    /// <summary>
    /// Bellus에선 사용하지 않음
    /// </summary>
    public override void Skill_Fourth()
    {}
}