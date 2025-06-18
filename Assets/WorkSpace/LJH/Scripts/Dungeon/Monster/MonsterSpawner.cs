using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class MonsterSpawner : MonoBehaviour
{
    [Inject] private DiContainer container;
    [Inject] private List<GameObject> monsters;

    [SerializeField] private List<GameObject> spawnerList = new List<GameObject>();
    [SerializeField] private List<GameObject> monsterList = new List<GameObject>();


    //플레이어 위치 체크 테스트용 변수
    [SerializeField] GameObject player;

    private Grid grid;
    BoundsInt localBounds;

    [SerializeField] Tilemap floor;

    int mapSize = 20;

    Dictionary<Direction, int> xPos = new Dictionary<Direction, int>();
    Dictionary<Direction, int> yPos = new Dictionary<Direction, int>();



    void Start()
    {
        Init();

        MonsterSpawnPosSet();
        MonsterSpawn();
    }

    private void Update()
    {
        MonsterActiver();
        
    }

    bool CheckTile(Vector3Int pos)
    {
        if (floor.GetTile(pos) != null)
        {
            return true;
        }

        return false;
    }
    int checkNum;
    void MonsterSpawnPosSet()
    {
        foreach (GameObject spawner in spawnerList)
        {
            checkNum = 0;
            Vector3 spawnPos = Vector3.zero;
            Vector3Int tilePos = Vector3Int.zero;
            do
            {
                int xPos = Random.Range(this.xPos[Direction.Left], this.xPos[Direction.Right]) + (int)GetComponentInParent<Transform>().position.x;
                int yPos = Random.Range(this.yPos[Direction.Up], this.yPos[Direction.Down]) + (int)GetComponentInParent<Transform>().position.y;

                spawnPos = new Vector3Int(xPos, yPos, 0);
                tilePos = floor.WorldToCell(spawnPos);
                checkNum++;
            } while (!CheckTile(tilePos) && checkNum < 30);

            if (checkNum == 30)
            {
                Debug.Log("무한루프 터짐");
            }

            spawner.transform.position = spawnPos;
        }
    }

    void MonsterSpawn()
    {
        for(int i = 0; i < spawnerList.Count; i++)
        {
            //젠젝트로 사용해야 하기에 컨테이너를 이용한 Instantiate 사용
            monsterList.Add(container.InstantiatePrefab
                (monsters[Random.Range(0, monsters.Count)], spawnerList[i].transform.position, Quaternion.identity, null));

            //초기 생성시 플레이어가 없는 방에서는 몬스터 비활성화
            MonsterActiver();
        }

        
    }

    /// <summary>
    /// 현재 플레이어가 존재하지 않는 방에선 몬스터 비활성화 함수
    /// 맵 이동시 해당 함수 넣어줘서 활성화 처리
    /// </summary>
    public void MonsterActiver()
    {
        foreach(GameObject mon in monsterList)
        {
            mon.SetActive(ContainsPlayer(player.transform.position));
        }
    }

    bool ContainsPlayer(Vector3 pos)
    {
        Vector3Int localCell = grid.WorldToCell(pos);
        return localBounds.Contains(localCell);
    }

    void Init()
    {
        //플레이어 참조용 테스트 코드
        player = GameObject.FindWithTag("Player");
        grid = GetComponentInParent<Grid>();
        localBounds = floor.cellBounds;

        xPos[Direction.Right] = -mapSize;
        xPos[Direction.Left] = mapSize;
        yPos[Direction.Up] = mapSize;
        yPos[Direction.Down] = -mapSize;

    }

}
