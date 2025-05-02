using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forge : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        UiOnOffMethod(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        UiOnOffMethod(collision);
    }

    public virtual void UiOnOffMethod(Collision2D collision) {}
}
