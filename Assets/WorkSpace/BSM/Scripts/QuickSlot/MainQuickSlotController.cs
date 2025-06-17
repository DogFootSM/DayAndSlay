using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainQuickSlotController : 
    MonoBehaviour, 
    IDragHandler, IDropHandler, IBeginDragHandler
{
    [SerializeField] private GraphicRaycaster mainQuickSlotGraphicRaycaster;
    [SerializeField] private Image mouseFollowIcon;
     
    private List<RaycastResult> raycastResults = new List<RaycastResult>();

    private QuickSlotManager quickSlotManager => QuickSlotManager.Instance;
    private SkillNode prevSkillNode;
    private QuickSlot prevQuickSlot;
    private QuickSlot nextQuickSlot;
    
    public void OnDrag(PointerEventData eventData)
    {
        if (prevSkillNode == null) return;
        if (!mouseFollowIcon.gameObject.activeSelf) MouseFollowIconActive(true);
        
        mouseFollowIcon.transform.position = eventData.position; 
    }

    public void OnDrop(PointerEventData eventData)
    { 
        if (mouseFollowIcon.gameObject.activeSelf) MouseFollowIconActive(false); 
        
        mainQuickSlotGraphicRaycaster.Raycast(eventData, raycastResults); 
        
        foreach (RaycastResult raycastResult in raycastResults)
        {
            nextQuickSlot = raycastResult.gameObject.GetComponentInParent<QuickSlot>();

            if (nextQuickSlot != null && nextQuickSlot != prevQuickSlot)
            {
                quickSlotManager.SwapSkillsInQuickSlots(prevQuickSlot, nextQuickSlot);
                break;
            } 
        } 
    }
 
    public void OnBeginDrag(PointerEventData eventData)
    {
        raycastResults.Clear();
        mainQuickSlotGraphicRaycaster.Raycast(eventData, raycastResults);
        UpdatePrevSkillNode();
    }
    
    /// <summary>
    /// 이동 전 슬롯의 스킬 노드 업데이트
    /// </summary>
    private void UpdatePrevSkillNode()
    {
        prevQuickSlot = raycastResults[0].gameObject.GetComponentInParent<QuickSlot>(); 
        prevSkillNode = prevQuickSlot.CurrentSlotSkillNode;

        if (prevSkillNode != null)
        {
            mouseFollowIcon.sprite = prevSkillNode.skillData.SkillIcon;
        }     
    }

    /// <summary>
    /// 드래그 스킬 아이콘 On, Off
    /// </summary>
    /// <param name="isActive">변경할 상태</param>
    private void MouseFollowIconActive(bool isActive)
    {
        mouseFollowIcon.gameObject.SetActive(isActive);
        
    }


}
