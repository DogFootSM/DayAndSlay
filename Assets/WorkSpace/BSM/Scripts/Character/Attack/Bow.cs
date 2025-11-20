using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : IAttackHandler
{
    private ArrowPool arrowPool => ArrowPool.Instance;
    
    public void NormalAttack(Vector2 direction, Vector2 position, ItemData itemData, PlayerModel playerModel)
    {
        GameObject arrowInstance = arrowPool.GetPoolArrow();
        arrowInstance.SetActive(true);
        arrowInstance.transform.parent = null;
        
        Arrow arrow = arrowInstance.GetComponent<Arrow>();
        arrow.SetLaunchTransform(position + (direction.normalized * 0.5f),direction, itemData.Range);
        
        arrow.SetArrowDamage(playerModel.FinalPhysicalDamage);
    }
}
