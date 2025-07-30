using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class NpcTalk : MonoBehaviour
{
    [SerializeField] private NpcTalkData npcTalkData;
    [SerializeField] private Animator bubbleAnimator;
    [SerializeField] private TextMeshProUGUI speechText;
    
    public UnityAction OnTalkPrintEvent;
    
    [SerializeField] private List<string> npcTalks = new List<string>();
    private Coroutine talkPrintCo;
    private TalkManager talkManager => TalkManager.Instance;
    
    private int bubbleAnimHash;
    private bool canTalkState = true;
    
    private void Awake()
    {
        npcTalkData = Resources.Load<NpcTalkData>("TalkData/NPCTalkData");
        bubbleAnimHash = Animator.StringToHash("PlayTalk");
    }

    private void Start()
    {
        npcTalks = talkManager.GetTalkData(GenderType.MALE, AgeType.CHILD);
    }

    private void OnEnable()
    {
        OnTalkPrintEvent += TalkPrint;
    }

    private void OnDisable()
    {
        OnTalkPrintEvent -= TalkPrint;
    }

    /// <summary>
    /// NPC 대사 출력
    /// </summary>
    private void TalkPrint()
    {
        int randIndex = Random.Range(0, npcTalks.Count);

        if (canTalkState)
        {
            if (talkPrintCo != null)
            {
                StopCoroutine(talkPrintCo);
                talkPrintCo = null;
            }

            talkPrintCo = StartCoroutine(TalkPrintRoutine(randIndex));
        }
         
    }

    /// <summary>
    /// NPC 대사 출력 코루틴
    /// </summary>
    /// <param name="randIndex">대사 리스트 중 출력할 대사 리스트의 인덱스</param>
    /// <returns></returns>
    private IEnumerator TalkPrintRoutine(int randIndex)
    {
        bubbleAnimator.SetBool(bubbleAnimHash, true);
        canTalkState = false;
        speechText.text = "";
        int talkIndex = 0;
        
        yield return new WaitUntil(() => bubbleAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);

        float elapsedTime = 0f;
        
        while (elapsedTime <= 0.5f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
            speechText.color = new Color(speechText.color.r, speechText.color.g, speechText.color.b, elapsedTime / 0.5f);  
        }
         
        while (talkIndex < npcTalks[randIndex].Length)
        {
            speechText.text += npcTalks[randIndex][talkIndex++];
            yield return WaitCache.GetWait(0.1f);
        }
        
        yield return WaitCache.GetWait(1.5f);

        elapsedTime = 0.5f;

        while (elapsedTime >= 0f)
        {
            elapsedTime -= Time.deltaTime;
            yield return null;
            speechText.color = new Color(speechText.color.r, speechText.color.g, speechText.color.b, elapsedTime / 0.5f);
        }
        
        bubbleAnimator.SetBool(bubbleAnimHash, false);
        canTalkState = true;
    }
    
}
