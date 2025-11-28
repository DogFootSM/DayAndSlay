using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class NpcSpawner : MonoBehaviour
{
    [Inject]
    DiContainer container;
    private Vector3 npcSpawnPos;

    [Header("NPC가 물건을 구매할 의향 있게 태어나는 확률 1 / npcBuyProbability")]
    [SerializeField] float npcBuyProbability = 3f;

    private float npcSpawnDelay = 5f;
    private WaitForSeconds delayTime;

    [Inject]
    private List<Npc> npcPreset = new List<Npc>();
    private List<Npc> npcList = new List<Npc>();
    void Start()
    {
        Init();
        
        //테스트시에만 활성화
        //StartCoroutine(NpcSpawnCoroutine());
    }

    public IEnumerator NpcSpawnCoroutine()
    {
        //낮될때마다 재실행 시켜줘야함
        while (DayManager.instance.dayOrNight == DayAndNight.DAY)
        {
            yield return delayTime;

            int npcIndex = Random.Range(0, npcPreset.Count);
            npcList.Add(container.InstantiatePrefabForComponent<Npc>(npcPreset[npcIndex], npcSpawnPos, Quaternion.identity,null));
            
            Npc npc = npcList[npcList.Count - 1];
            
            //npc의 타입 설정
            //GenderType gender = (GenderType)Random.Range(0, 2);
            //AgeType age = (AgeType)Random.Range(0, 3);
            //npc.SetNpcType(gender, age);
            
            //npc의 바이어 y/n 설정
            npc.IsBuyer = Random.value < 1f / npcBuyProbability;
            npc.name = npc.name.Replace("(Clone)", "");
        }
    }

    void Init()
    {
        npcSpawnPos = transform.position;

        delayTime = new WaitForSeconds(npcSpawnDelay);
    }
    
}
