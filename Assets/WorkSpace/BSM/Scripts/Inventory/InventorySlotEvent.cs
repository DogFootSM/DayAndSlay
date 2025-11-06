using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotEvent : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static Action<PointerEventData> OnSlotBeginDrag;
    public static Action<PointerEventData> OnSlotEndDrag;
    public static Action<PointerEventData> OnSlotDrag;


    public void OnBeginDrag(PointerEventData eventData)
    {
        OnSlotBeginDrag?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnSlotDrag?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnSlotEndDrag?.Invoke(eventData);
    }
}
