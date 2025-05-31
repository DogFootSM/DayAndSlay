using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackHandler
{
    public void NormalAttack(Vector2 direction, Vector2 position) {}
    public void DrawGizmos(){}
}
