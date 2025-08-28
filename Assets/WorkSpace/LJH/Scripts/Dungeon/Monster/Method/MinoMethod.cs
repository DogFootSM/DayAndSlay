using UnityEngine;

public class MinoMethod : BossMonsterMethod
{
    private BossMonsterAI mino;
    
    [SerializeField] private ParticleSystem stompEffect;

    public override void Skill_First()
    {
        
    }

    public override void Skill_Second()
    {
        //SetPosEffect(stompEffect, gameObject);
        //EffectPlay(stompEffect);
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
