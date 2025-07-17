using System;
using System.Collections;
using System.Collections.Generic;
using Nobi.UiRoundedCorners;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainQuickSlot : MonoBehaviour,
    IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Image skillIconImage;
    [SerializeField] private TextMeshProUGUI quickSlotTypeText;
    [SerializeField] private QuickSlotType curQuickSlotType;
    [SerializeField] private GraphicRaycaster mainQuickSlotRaycaster;
    [SerializeField] private Image beginDragImage;
    [SerializeField] private SkillCoolDown skillCoolDown;
    [SerializeField] public Image skillIconRadiusParent;
    
    public QuickSlotType CurrentQuickSlot => curQuickSlotType;
    public Image SkillIconImage => skillIconImage;
    
    
    private List<RaycastResult> raycastResults = new List<RaycastResult>();
    private MainQuickSlot beginSlot;
    private QuickSlotManager quickSlotManager => QuickSlotManager.Instance;
    
    private void Awake()
    {
        quickSlotTypeText.text = $"{curQuickSlotType}"; 
        
        CoolDownUIHub.CoolDownUIRegistry(curQuickSlotType, skillCoolDown);
    }

    /// <summary>
    /// 메인 퀵슬롯 UI 정보 설정
    /// </summary>
    /// <param name="skillNode"></param>
    public void SetMainQuickSlot(SkillNode skillNode = null)
    {
        if (skillNode == null)
        {
            skillIconRadiusParent.color = new Color(1f, 1f, 1f, 0f);
            skillIconImage.sprite = null;
        }
        else
        {
            skillIconRadiusParent.color = new Color(1f, 1f, 1f, 1f);
            skillIconImage.sprite = skillNode.skillData.SkillIcon; 
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        raycastResults.Clear();
        
        mainQuickSlotRaycaster.Raycast(eventData, raycastResults);
        
        foreach (RaycastResult raycastResult in raycastResults)
        {
            beginSlot = raycastResult.gameObject.GetComponentInParent<MainQuickSlot>();
        }

        if (beginSlot == null) return;
        
        if (QuickSlotData.WeaponQuickSlotDict[quickSlotManager.CurrentWeaponType].ContainsKey(beginSlot.CurrentQuickSlot))
        {
            beginDragImage.gameObject.SetActive(true);
            beginDragImage.sprite = beginSlot.SkillIconImage.sprite; 
        } 
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (beginSlot == null) return;
        
        beginDragImage.transform.position = eventData.position; 
    }   

    public void OnEndDrag(PointerEventData eventData)
    {
        if(beginSlot == null) return;
        
        if (!QuickSlotData.WeaponQuickSlotDict[quickSlotManager.CurrentWeaponType].ContainsKey(beginSlot.CurrentQuickSlot)) return;
        
        mainQuickSlotRaycaster.Raycast(eventData, raycastResults);

        foreach (RaycastResult raycastResult in raycastResults)
        {
            MainQuickSlot endSlot = raycastResult.gameObject.GetComponentInParent<MainQuickSlot>();

            if (endSlot != null && beginSlot != endSlot)
            {
                quickSlotManager.SlotSwapRequest(beginSlot.CurrentQuickSlot, endSlot.CurrentQuickSlot);
                break;
            } 
        } 
        
        beginDragImage.gameObject.SetActive(false);
    }
}