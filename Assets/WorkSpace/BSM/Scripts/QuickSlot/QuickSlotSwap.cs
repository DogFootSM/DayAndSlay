using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlotSwap : MonoBehaviour
{
    /// <summary>
    /// 스킬 노드가 할당되어 있는 퀵슬롯 스왑
    /// </summary>
    /// <param name="beginSlot">처음 감지한 퀵슬롯</param>
    /// <param name="endSlot">처음 감지한 퀵슬롯과 스왑할 퀵슬롯</param>
    /// <param name="weaponType">현재 장착중인 무기 타입을 키 값으로 사용</param>
    public void QuickSlotSkillNodeSwap(QuickSlotType beginSlot, QuickSlotType endSlot, CharacterWeaponType weaponType)
    {
        //이동할 슬롯에 스킬이 등록 안된 경우
        if (!QuickSlotData.WeaponQuickSlotDict[weaponType].ContainsKey(endSlot))
        {
            SkillNode tempNode = QuickSlotData.WeaponQuickSlotDict[weaponType][beginSlot];

            QuickSlotData.WeaponQuickSlotDict[weaponType].Add(endSlot, tempNode);
            QuickSlotData.BeforeQuickSlotTypeDict[tempNode] = endSlot;

            QuickSlotData.WeaponQuickSlotDict[weaponType].Remove(beginSlot);
        }
        else
        {
            //Begin => End, End => Begin 서로 스왑
            (QuickSlotData.WeaponQuickSlotDict[weaponType][beginSlot],
                    QuickSlotData.WeaponQuickSlotDict[weaponType][endSlot])
                = (QuickSlotData.WeaponQuickSlotDict[weaponType][endSlot],
                    QuickSlotData.WeaponQuickSlotDict[weaponType][beginSlot]);

            SkillNode beginNode = QuickSlotData.WeaponQuickSlotDict[weaponType][beginSlot];
            SkillNode endNode = QuickSlotData.WeaponQuickSlotDict[weaponType][endSlot];
            
            //할당된 슬롯 갱신
            QuickSlotData.BeforeQuickSlotTypeDict[beginNode] = beginSlot;
            QuickSlotData.BeforeQuickSlotTypeDict[endNode] = endSlot;
        } 
    } 
}