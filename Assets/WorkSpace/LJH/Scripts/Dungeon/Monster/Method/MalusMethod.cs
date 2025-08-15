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
        //소환? 뭘로하지?
    }

    public override void Skill_Second()
    {
        SetPosEffect(warningRoot);
        SetPosEffect(root);
        
        SetEffectActiver(warningRoot);
        SetEffectActiver(root);
    }
}