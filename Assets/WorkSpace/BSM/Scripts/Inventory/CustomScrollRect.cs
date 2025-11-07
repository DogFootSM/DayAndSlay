using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomScrollRect : ScrollRect
{
    public static bool allowDrag;

    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (allowDrag) return;
        base.OnInitializePotentialDrag(eventData); 
    }
    
    public override void OnBeginDrag(PointerEventData eventData)
    {
        //스크롤뷰의 드래그 시작 동작 방지
        if (allowDrag)
        {
            eventData.pointerDrag = null;
            return;
        }
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        //스크롤뷰의 드래그 동작 방지
        if (allowDrag)
        {
            return;
        }
        base.OnDrag(eventData);
    }
    
    public override void OnEndDrag(PointerEventData eventData)
    {
        //스크롤뷰의 드래그 끝 동작 방지
        if(allowDrag) return;
        base.OnEndDrag(eventData);
    } 
}
