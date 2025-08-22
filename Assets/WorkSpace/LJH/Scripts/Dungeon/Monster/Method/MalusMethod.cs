using UnityEngine;

public class MalusMethod : BossMonsterMethod
{
    private BossMonsterAI malus;
    private BossMonsterAI bellus;

    [SerializeField] private GameObject warningRoot;
    [SerializeField] private GameObject root;

    private void Start()
    {
        malus = GetComponent<Malus>();
        bellus = ((Malus)malus).GetPartner();
    }
    public override void Skill_First()
    {
        //Todo : Àâ¸÷ ¼ÒÈ¯
    }

    public override void Skill_Second()
    {
        //Todo : »Ñ¸® °ø°Ý
    }
}