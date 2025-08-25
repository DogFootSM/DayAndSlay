using System.Collections;
using UnityEngine;
using Zenject;

public abstract class BossMonsterMethod : NewMonsterMethod
{
    public ParticleSystem effect;
    
    
    public abstract override void Skill_First();
    public abstract override void Skill_Second();
    
    public void SetPosEffect(ParticleSystem effect, GameObject target)
    {
        effect.transform.position = target.transform.position;
    }

    public void EffectPlay()
    {
        Debug.Log("실행됨");

        if (effect == null)
        {
            Debug.Log("effect가 Null입니다.");
        }
        effect.Play(true);
    }
}