using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObj : MonoBehaviour
{
    public abstract void Interaction();

    public void OnCollisionEnter2D(Collision2D collision)
    {
        UiOnOffMethod(collision);
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        UiOnOffMethod(collision);
    }

    public virtual void UiOnOffMethod(Collision2D collision) {}


}
