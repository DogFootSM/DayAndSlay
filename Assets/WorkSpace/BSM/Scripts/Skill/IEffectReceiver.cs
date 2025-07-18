using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectReceiver
{
    public void TakeDamage(float damage);
    public void ReceiveKnockBack(Vector2 playerPos, Vector2 playerDir);
    public void ReceiveDot(float duration, float tick, float damage);
    public void ReceiveStun(float duration);

}
