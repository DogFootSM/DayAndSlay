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

    [SerializeField] protected Tilemap floorTilemap;
    [SerializeField] protected Tilemap wallTilemap;


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

    virtual public void MonsterSpawnPosSet()
    {
        Room room = GetComponentInParent<Room>();
        GameObject _room = room.gameObject;
        
        if (room.GetBossCheck()) return;
        
        foreach (GameObject spawner in spawnerList)
        {
            if (floorTilemap == null) continue;

            List<Vector3> floorPositions = new List<Vector3>();
            
            BoundsInt bounds = floorTilemap.cellBounds;
            
            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                if (floorTilemap.HasTile(pos) && !wallTilemap.HasTile(pos))
                {
                    // Cell -> World 좌표 변환
                    Vector3 worldPos = floorTilemap.CellToWorld(pos) + floorTilemap.cellSize / 2f;
                    floorPositions.Add(worldPos);
                }
            }

            if (floorPositions.Count > 0)
            {
                Vector3 spawnerPos = floorPositions[Random.Range(0, floorPositions.Count)];

                spawner.transform.position = spawnerPos;
            }
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
            
            monster.GetComponentInChildren<TargetSensor>().SetGrid(grid);

            monsterList.Add(monster);
        }
        
        MonsterActiver();


        
    }

    private void SpawnerDestroy()
    {
        for (int i = spawnerList.Count - 1; i >= 0; i--)
        {
            //Destroy(spawnerList[i]);
        }
    }

    /// <summary>
    /// 현재 플레이어가 존재하지 않는 방에선 몬스터 비활성화 함수
    /// 맵 이동시 해당 함수 넣어줘서 활성화 처리
    /// </summary>
    public void MonsterActiver()
    {
        bool playerInside = ContainsPlayer(player.transform.position);
        Debug.Log(playerInside);

        foreach (GameObject mon in monsterList)
        {
            GridReFerence(mon);             // 그리드/타일 참조 갱신
            mon.SetActive(playerInside);    // 방 안이면 전부 On, 밖이면 Off
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
    
    private bool ContainsPlayer(Vector3 worldPos)
    {
        if (floor == null) return false;

        Vector3Int cell = floor.WorldToCell(worldPos);

        // bounds 체크 및 실제 바닥 타일 존재 체크
        if (!floor.cellBounds.Contains(cell)) return false;
        return floor.HasTile(cell);
    }

    private void Init()
    {
        player = GameObject.FindWithTag("Player");
        grid = GetComponent<Grid>();
        localBounds = floor.cellBounds;

        xPos[Direction.Left] = -mapSize;
        xPos[Direction.Right] = mapSize;
        yPos[Direction.Down] = -mapSize;
        yPos[Direction.Up] = mapSize;

        prevPlayerCellPos = grid.WorldToCell(player.transform.position); // 초기 위치 저장
    }


}
