using UnityEngine;

public class MalusMethod : BossMonsterMethod
{
    private BossMonsterAI malus;
    private BossMonsterAI bellus;

    [SerializeField] private GameObject warningRoot;
    [SerializeField] private GameObject root;

    public override void Skill_Fourth()
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        malus = GetComponent<MalusAI>();
        bellus = ((MalusAI)malus).GetPartner();
    }
    public override void Skill_First()
    {
        //Todo : Àâ¸÷ ¼ÒÈ¯
    }

    public override void Skill_Second()
    {
        //Todo : »Ñ¸® °ø°Ý
    }

    public override void Skill_Third()
    {
        throw new System.NotImplementedException();
    }
}