using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushableObject
{
    void PushableModeOn(Rigidbody2D rigid)
    {
        rigid.bodyType = RigidbodyType2D.Dynamic;
    }

    void PushableModeOff(Rigidbody2D rigid)
    {
        rigid.bodyType = RigidbodyType2D.Kinematic;
    }
}
