using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAS008 : MeleeSkill
{
    public SPAS008(SkillNode skillNode) : base(skillNode)
    {
    }
    
    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        skillNode.IsMarkOnTarget = false;
        IEffectReceiver receiver = skillNode.GetMarkOnTarget().GetComponent<IEffectReceiver>();
        receiver.ReceiveMarkOnTarget();
        ExecuteBlinkToMarkedTarget(skillNode.GetMarkOnTarget());
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType){}

    public override void Gizmos(){}
}
