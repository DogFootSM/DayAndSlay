using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using ModestTree.Util;
using UnityEngine;

public class BuffIconController : MonoBehaviour
{
    [SerializeField] private GameObject buffIconPrefab;
    
    [SerializedDictionary("Type", "Icon Image")]
    [SerializeField] private SerializedDictionary<BuffType, Sprite> buffIcons;
    
    /// <summary>
    /// 버프 or 회피기 사용 시 쿨다운 UI 리셋 진행을 알림
    /// 최초 사용일 경우 생성 후 UI 등록
    /// 재사용 상태일 경우 등록된 타입을 찾아서 실행
    /// </summary>
    /// <param name="buffType">사용한 버프 or 회피기 타입</param>
    /// <param name="coolDownDuration">쿨다운 시간</param>
    public void UseBuff(BuffType buffType, float coolDownDuration)
    {
        if (!CoolDownUIHub.BuffCoolDownMap.ContainsKey(buffType))
        {
            BuffIconInstantiate(buffType, coolDownDuration);
        }
        else
        {
            CoolDownUIHub.BuffCoolDownMap[buffType].gameObject.SetActive(true);
            CoolDownUIHub.BuffCoolDownMap[buffType].ResetCoolDown(coolDownDuration);
        }
    }
    
    /// <summary>
    /// 최초 버프 or 회피기 사용 시 아이콘 이미지 및 UI 등록
    /// </summary>
    /// <param name="buffType"></param>
    /// <param name="coolDownDuration"></param>
    private void BuffIconInstantiate(BuffType buffType, float coolDownDuration)
    {
        GameObject instance = Instantiate(buffIconPrefab, transform.position, Quaternion.identity, transform);
        instance.SetActive(true);
        
        BuffCoolDown buffCoolDown = instance.GetComponent<BuffCoolDown>();
        buffCoolDown.SetIconImage(buffIcons[buffType]);
        
        CoolDownUIHub.BuffCoolDownUIRegistry(buffType, buffCoolDown);
        CoolDownUIHub.BuffCoolDownMap[buffType].ResetCoolDown(coolDownDuration);
    }
}