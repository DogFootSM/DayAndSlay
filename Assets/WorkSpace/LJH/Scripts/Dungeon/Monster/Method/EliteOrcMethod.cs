using UnityEngine;

public class EliteOrcMethod : BossMonsterMethod
{
    private BossMonsterAI eliteOrc;
    
    [SerializeField] private GameObject rushEffect;

    private void Start()
    {
        MonsterInit();
    }
    public override void Skill_First()
    {
        //돌진 스킬
        SetEffectActiver(rushEffect);
        
    }

    public override void Skill_Second()
    {
    }

}
