using Unity.VisualScripting;
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
        //UiOnOffMethod(collision);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        UiOnOffMethod(collider);
    }

    public virtual void UiOnOffMethod(Collision2D collision) {}

    public virtual void UiOnOffMethod(Collider2D collider) {}


}
