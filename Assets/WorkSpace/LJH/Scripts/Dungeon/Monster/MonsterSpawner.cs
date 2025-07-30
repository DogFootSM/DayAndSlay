using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class MonsterSpawner : MonoBehaviour
{
    [Inject] protected DiContainer container;
    [Inject(Id = "MONSTER")] protected List<GameObject> monsters;
    [Inject(Id = "BOSS")] protected List<GameObject> bossMonsters;

    [SerializeField] protected List<GameObject> spawnerList = new List<GameObject>();
    [SerializeField] protected List<GameObject> monsterList = new List<GameObject>();


    //플레이어 위치 체크 테스트용 변수
    [SerializeField] private GameObject player;

    private Grid grid;
    private BoundsInt localBounds;

    [SerializeField] protected Tilemap floor;

    private int mapSize = 20;

    private Dictionary<Direction, int> xPos = new Dictionary<Direction, int>();
    private Dictionary<Direction, int> yPos = new Dictionary<Direction, int>();
    
    private Vector3Int prevPlayerCellPos;
    
    private bool IsBossRoom()
    {
        return grid.name == "Grid 5";
    }

    private void Start()
    {
        StartCoroutine(DelayCoroutine());
    }

    IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        Init();
        MonsterSpawnPosSet();
        MonsterSpawn();
        SpawnerDestroy();
    }

    private void Update()
    {
        if (player == null) return;
        
        Vector3Int currentCell = grid.WorldToCell(player.transform.position);
        if (currentCell != prevPlayerCellPos)
        {
            prevPlayerCellPos = currentCell;
            MonsterActiver(); // 위치 바뀌었을 때만 호출
        }
        
    }

    private bool CheckTile(Vector3Int pos)
    {
        if (floor.GetTile(pos) != null)
        {
            return true;
        }

        return false;
    }
    int checkNum;
    virtual public void MonsterSpawnPosSet()
    {
        if (IsBossRoom()) return;

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

    virtual public void MonsterSpawn()
    {
        for (int i = 0; i < spawnerList.Count; i++)
        {
            GameObject prefabToSpawn;

            if (IsBossRoom())
            {
                prefabToSpawn = bossMonsters[Random.Range(0, bossMonsters.Count)];
            }
            else
            {
                prefabToSpawn = monsters[Random.Range(0, monsters.Count)];
            }

            GameObject monster = container.InstantiatePrefab(
                prefabToSpawn, spawnerList[i].transform.position, Quaternion.identity, null);

            monsterList.Add(monster);
        }
        
        MonsterActiver();


        
    }

    private void SpawnerDestroy()
    {
        for (int i = spawnerList.Count - 1; i >= 0; i--)
        {
            Destroy(spawnerList[i]);
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
            GridReFerence(mon);
            mon.SetActive(ContainsPlayer(player.transform.position));
        }
    }

    /// <summary>
    /// 그리드 참조해주는 함수
    /// </summary>
    private void GridReFerence(GameObject mon)
    {
        //AstarPath 내부에서 그리드 지정해주는
        TargetSensor targetSensor = mon.GetComponentInChildren<TargetSensor>();
        AstarPath astarPath = mon.GetComponentInChildren<AstarPath>();

        targetSensor.grid = grid;
        astarPath.mapGrid = grid;
        astarPath.TileMapReference();
    }

    private bool ContainsPlayer(Vector3 pos)
    {
        Vector3Int localCell = grid.WorldToCell(pos);
        return localBounds.Contains(localCell);
    }

    private void Init()
    {
        player = GameObject.FindWithTag("Player");
        grid = GetComponentInParent<Grid>();
        localBounds = floor.cellBounds;

        xPos[Direction.Left] = -mapSize;
        xPos[Direction.Right] = mapSize;
        yPos[Direction.Down] = -mapSize;
        yPos[Direction.Up] = mapSize;

        prevPlayerCellPos = grid.WorldToCell(player.transform.position); // 초기 위치 저장
    }


}
