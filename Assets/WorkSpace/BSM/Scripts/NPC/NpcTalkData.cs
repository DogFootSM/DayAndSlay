using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  
[CreateAssetMenu(menuName = "Data/NPCTalkData", fileName = "NPCTalkData")]
public class NpcTalkData : ScriptableObject
{
    private Dictionary<(GenderType, AgeType), List<string>> talkMaps = new Dictionary<(GenderType, AgeType), List<string>>();
    
    /// <summary>
    /// NPC Talk Data 파싱
    /// </summary>
    /// <param name="gender">키값으로 사용할 성별 타입</param>
    /// <param name="ageType">키값으로 사용할 나이 타입</param>
    /// <param name="talkData">출력할 대사</param>
    public void SetParseTalkData(GenderType gender, AgeType ageType, string talkData)
    {
        var key = (gender, ageType);
        
        if (!talkMaps.ContainsKey(key))
        {
            talkMaps[key] = new List<string>();
        }
        
        talkMaps[key].Add(talkData);
    }
    
    /// <summary>
    /// 성별과 나이에 따른 대사 반환
    /// </summary>
    /// <returns></returns>
    public List<string> GetTalkData(GenderType gender, AgeType age)
    {
        var key = (gender, age);

        if (!talkMaps.ContainsKey(key))
        {
            talkMaps[key] = new List<string>();
        }
        
        return talkMaps[key];
    }
    
}
