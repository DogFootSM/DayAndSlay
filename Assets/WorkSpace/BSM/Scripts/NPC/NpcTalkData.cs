using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  
[CreateAssetMenu(menuName = "Data/NPCTalkData", fileName = "NPCTalkData")]
public class NpcTalkData : ScriptableObject
{
    public GenderType genderType;
    public AgeType ageType;
    public string talk;

    /// <summary>
    /// NPC Talk Data 파싱
    /// </summary>
    public void SetParseTalkData(GenderType genderType, AgeType ageType, string talk)
    {
        this.genderType = genderType;
        this.ageType = ageType;
        this.talk = talk;
    }
}
