using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackHandler
{
    public void SetDirection(Vector2 direction){}
    public void NormalAttack(Vector2 position, ItemData itemData, PlayerModel playerModel) {}
    public void DrawGizmos(){}
}
