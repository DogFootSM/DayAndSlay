using System.Collections;
using UnityEngine;
using Zenject;

public abstract class BossMonsterMethod : NewMonsterMethod
{
    public abstract override void Skill_First();
    public abstract override void Skill_Second();
    
    protected void SetPosEffect(GameObject effect, GameObject target)
    {
        effect.transform.position = target.transform.position;
    }

    protected void SetEffectActiver(GameObject effect)
    {
        effect.SetActive(true);
    }
}