using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    [SerializeField] private List<NpcTalkData> npcTalkData;
    
    public static TalkManager Instance;
     
    private Dictionary<(GenderType, AgeType), List<string>> talkMaps = new Dictionary<(GenderType, AgeType), List<string>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SetParseTalkData();
    }

    /// <summary>
    /// NPC Talk Data 키 설정
    /// </summary> 
    private void SetParseTalkData()
    {
        for (int i = 0; i < npcTalkData.Count; i++)
        {
            var key = (genderType: npcTalkData[i].CurGenderType, ageType: npcTalkData[i].CurAgeType);
            
            if (!talkMaps.ContainsKey(key))
            {
                talkMaps[key] = new List<string>();
            }
            
            talkMaps[key].Add(npcTalkData[i].Talk);
        }
    }
    
    /// <summary>
    /// 성별과 나이에 따른 대사 반환
    /// </summary>
    /// <returns></returns>
    public List<string> GetTalkData(GenderType gender, AgeType age)
    { 
        var key = (gender, age);

        return talkMaps[key];
    }
}
