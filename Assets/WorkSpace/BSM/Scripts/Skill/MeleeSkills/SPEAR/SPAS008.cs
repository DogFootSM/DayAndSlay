using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;

public class SPAS008 : MeleeSkill
{
    private Action action;
    
    public SPAS008(SkillNode skillNode) : base(skillNode)
    {
        leftHash = Animator.StringToHash("LeftAttack3");
        rightHash = Animator.StringToHash("RightAttack3");
        upHash = Animator.StringToHash("UpAttack3");
        downHash = Animator.StringToHash("DownAttack3");
    }
    
    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    { 
        ListClear();
        //타겟 위치로 이동 후 표식 설정 Off
        skillNode.IsMarkOnTarget = false;
        
        //마킹한 타겟 Collider 확인, 마킹 타겟이 없을 경우 Null
        IEffectReceiver receiver = skillNode.GetMarkOnTarget().GetComponent<IEffectReceiver>();
        receiver.ReceiveMarkOnTarget();

        action = () => RegisterAction(receiver, playerPosition);

        //타겟의 Collider 위치로 이동
        ExecuteBlinkToMarkedTarget(skillNode.GetMarkOnTarget(), action);
    }

    private void RegisterAction(IEffectReceiver receiver, Vector2 playerPosition)
    {
        SkillEffect(playerPosition, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        ListClear();
        
        Vector2 dir = (Vector2)skillNode.GetMarkOnTarget().transform.position - playerPosition;
        skillDamage = GetSkillDamage();
        
        SkillEffect(skillNode.GetMarkOnTarget().transform.position, 0, $"{skillNode.skillData.SkillId}_2_Particle", skillNode.skillData.SkillEffectPrefab[1]);
        Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount);

        //Radian -> Degree로 변경 후 -270도 보정
        float degree = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 270f;
        instance.transform.localRotation = Quaternion.Euler(0, 0, degree);
         
        // 스프라이트 이미지에 따라 radian 값을 -270도 보정해줌
        // float deg = Mathf.Atan2(directionVector.y, directionVector.x) - 270f * Mathf.Deg2Rad;
        // mainModule = mainModules[0];
        // mainModule.startRotationZ = deg; 
    }
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType){}

    public override void Gizmos(){}
}
