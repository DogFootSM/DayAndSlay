using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : IAttackHandler
{
    public void NormalAttack(Vector2 direction, Vector2 position, ItemData itemData, PlayerModel playerModel)
    {
        Debug.Log("완드 일반 공격");
    }
}
