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

}
