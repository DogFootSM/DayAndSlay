using UnityEngine;

public class MinoMethod : BossMonsterMethod
{
    private BossMonsterAI mino;
    
    [SerializeField] private GameObject stompEffect;

    public override void Skill_First()
    {
        
    }

    public override void Skill_Second()
    {
        //SetEffectActiver(stompEffect);
    }

}
