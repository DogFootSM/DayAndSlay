using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NpcTalk : MonoBehaviour
{
    [SerializeField] private NpcTalkData npcTalkData;

    public UnityAction OnTalkPrintEvent;
    
    private List<string> npcTalks = new List<string>();
    
    private void Awake()
    {
        npcTalkData = Resources.Load<NpcTalkData>("TalkData/NPCTalkData");
        
        npcTalkData.SetParseTalkData(GenderType.MALE, AgeType.CHILD, "이거 진짜 미친놈이네");
        npcTalkData.SetParseTalkData(GenderType.MALE, AgeType.CHILD, "개패고싶다 진짜");
        npcTalkData.SetParseTalkData(GenderType.MALE, AgeType.CHILD, "ㅋㅋ또 쳐늦네");
        npcTalkData.SetParseTalkData(GenderType.MALE, AgeType.CHILD, "밤에 뭘 하는거지 도대체?");
        npcTalkData.SetParseTalkData(GenderType.FEMALE, AgeType.CHILD, "하 씨발");
        npcTalkData.SetParseTalkData(GenderType.FEMALE, AgeType.SENIOR, "뻑");

        npcTalks = npcTalkData.GetTalkData(GenderType.MALE, AgeType.CHILD);

    }

    private void OnEnable()
    {
        OnTalkPrintEvent += TalkPrint;
    }

    private void OnDisable()
    {
        OnTalkPrintEvent -= TalkPrint;
    }

    private void TalkPrint()
    {
        int randIndex = Random.Range(0, npcTalks.Count);
        
        Debug.Log(npcTalks[randIndex]);
    }
    
}
