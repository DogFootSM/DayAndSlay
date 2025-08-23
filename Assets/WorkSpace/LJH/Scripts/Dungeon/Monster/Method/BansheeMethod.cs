using UnityEngine;

public class BansheeMethod : BossMonsterMethod
{
    private BossMonsterAI benshee;
    
    [SerializeField] private GameObject stompEffect;

    public override void Skill_First()
    {
        
    }

    public override void Skill_Second()
    {
        //SetEffectActiver(stompEffect);
    }

}
