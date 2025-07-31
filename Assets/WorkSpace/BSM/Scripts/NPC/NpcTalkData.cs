using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "Data/NPCTalkData", fileName = "NPCTalkData")]
public class NpcTalkData : ScriptableObject
{

    private GenderType curGender;
    private AgeType curAge;
    private string printTalk;
    
    
    public GenderType CurGenderType => curGender;
    public AgeType CurAgeType => curAge;
    public string Talk => printTalk;

    /// <summary>
    /// NPC Talk Data 파싱
    /// </summary>
    public void SetParseTalkData(GenderType genderType, AgeType ageType, string talk)
    {
        this.curGender = genderType;
        this.curAge = ageType;
        this.printTalk = talk;
    }
}
