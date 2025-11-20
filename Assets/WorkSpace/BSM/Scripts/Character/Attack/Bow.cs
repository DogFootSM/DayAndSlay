using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : IAttackHandler
{
    private ArrowPool arrowPool => ArrowPool.Instance;
    
    public void NormalAttack(Vector2 direction, Vector2 position, ItemData itemData, PlayerModel playerModel)
    {
        Debug.Log("활 기본 공격");
        
        GameObject arrowInstance = arrowPool.GetPoolArrow();
        arrowInstance.SetActive(true);
        arrowInstance.transform.parent = null;
        Arrow arrow = arrowInstance.GetComponent<Arrow>();
        //TODO: 장착 무기의 사거리를 전달 필요
        arrow.SetLaunchTransform(position + (direction.normalized * 0.5f),direction, 3f);
        
        //TODO: 캐릭터 레벨 및 스탯에 따른 데미지 계산 적용
        arrow.SetArrowDamage(5f);
    }
}
