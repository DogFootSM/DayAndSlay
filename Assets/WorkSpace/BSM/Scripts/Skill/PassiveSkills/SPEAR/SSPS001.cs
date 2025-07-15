using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSPS001 : PassiveSkill
{
    public SSPS001(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    { 
    }

    public override void ApplyPassiveEffects()
    {
        PassiveEffect();
        Debug.Log($"{skillNode.skillData.SkillName} 패시브 스킬");
        skillNode.PlayerModel.ApplyPassiveSkillModifiers(); 
        
        //TODO: 패시브 스킬 효과 적용을 여기서 구현? PlayerStats를 반환 받아서 작업?
        //레벨업 능력치 투자처럼 playerStat과 Visible 스탯을 조정?
        //statusWindow.OnChangedAllStats?.Invoke(playerStats); 이벤트 발동시켜줘야 하나?
    }

    public override void Gizmos()
    {
        throw new System.NotImplementedException();
    }
}
