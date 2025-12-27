using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : IAttackHandler
{
    private ArrowPool arrowPool => ArrowPool.Instance;
    
    private Vector2 curDirection;
    
    /// <summary>
    /// 공격 방향 설정
    /// </summary>
    /// <param name="direction"></param>
    public void SetDirection(Vector2 direction)
    {
        curDirection = direction;
    }
    
    public void NormalAttack(Vector2 position, ItemData itemData, PlayerModel playerModel)
    {
        GameObject arrowInstance = arrowPool.GetPoolArrow();
        arrowInstance.SetActive(true);
        arrowInstance.transform.parent = null;

        Arrow arrow = arrowInstance.GetComponent<Arrow>();
        arrow.SetLaunchTransform(position + (curDirection.normalized * 0.5f),curDirection, itemData.Range);
        
        arrow.SetArrowDamage(playerModel.FinalPhysicalDamage);
    }

    public void BackDashAttack(Vector2 position, Vector2 direction, ItemData itemData, PlayerModel playerModel)
    {
        GameObject arrowInstance = arrowPool.GetPoolArrow();
        arrowInstance.SetActive(true);
        arrowInstance.transform.parent = null;

        Arrow arrow = arrowInstance.GetComponent<Arrow>();
        arrow.SetLaunchTransform(position + (direction.normalized * 0.5f),direction, itemData.Range);
        
        arrow.SetArrowDamage(playerModel.FinalPhysicalDamage);
    }
    
}
