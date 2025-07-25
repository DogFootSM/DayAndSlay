using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : IAttackHandler
{
    public void NormalAttack(Vector2 direction, Vector2 position)
    {
        Debug.Log("활 일반 공격");
    }
    
}
