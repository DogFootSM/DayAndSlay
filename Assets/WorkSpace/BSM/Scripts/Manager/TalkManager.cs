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
            var key = (npcTalkData[i].genderType, npcTalkData[i].ageType);
            
            if (!talkMaps.ContainsKey(key))
            {
                talkMaps[key] = new List<string>();
            }
            
            talkMaps[key].Add(npcTalkData[i].talk);
        } 
    }
    
    /// <summary>
    /// 성별과 나이에 따른 대사 반환
    /// </summary>
    /// <returns></returns>
    public List<string> GetTalkData(GenderType gender, AgeType age)
    {
        Debug.Log($"{gender} - {age}");
        
        var key = (gender, age);

        if (!talkMaps.ContainsKey(key))
        {
            talkMaps[key] = new List<string>();
        }
        
        return talkMaps[key];
    }
}
