using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private List<NpcNew> npcPreset = new List<NpcNew>();
    private List<NpcNew> npcList = new List<NpcNew>();
    void Start()
    {
        Init();

        StartCoroutine(NpcSpawnCoroutine());
    }

    IEnumerator NpcSpawnCoroutine()
    {
        //낮될때마다 재실행 시켜줘야함
        while (DayManager.instance.dayOrNight == DayAndNight.DAY)
        {
            yield return delayTime;

            int npcIndex = Random.Range(0, npcPreset.Count);
            npcList.Add(container.InstantiatePrefabForComponent<NpcNew>(npcPreset[npcIndex], npcSpawnPos, Quaternion.identity,null));
            npcList[npcList.Count - 1].IsBuyer = Random.value < 1f / npcBuyProbability;
            npcList[npcList.Count - 1].name = $"npc{npcList.Count - 1}";
        }
    }

    void Init()
    {
        npcSpawnPos = transform.position;

        delayTime = new WaitForSeconds(npcSpawnDelay);
    }
    
}
