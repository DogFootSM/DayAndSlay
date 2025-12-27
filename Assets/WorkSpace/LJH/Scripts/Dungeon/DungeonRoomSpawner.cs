using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class DungeonRoomSpawner : MonoBehaviour
{
    [Inject] protected DiContainer container;

    [SerializeField] private DungeonPathfinder dungeonPathfinder;

    [SerializeField] [SerializedDictionary]
    private SerializedDictionary<string, List<GameObject>> roomsDict;

    [SerializeField] [SerializedDictionary]
    private SerializedDictionary<string, List<GameObject>> bossRoomsDict;

    [SerializeField] private List<GameObject> rooms_Stage1;
    [SerializeField] private List<GameObject> rooms_Stage2;
    [SerializeField] private List<GameObject> rooms_Stage3;

    [SerializeField] private List<GameObject> bossRooms_Stage1;
    [SerializeField] private List<GameObject> bossRooms_Stage2;
    [SerializeField] private List<GameObject> bossRooms_Stage3;

    [SerializeField] private List<GameObject> roomPos;

    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private GameObject doorMarker;

    public static StageNum stageNum;

    public List<GameObject> RoomList = new List<GameObject>();
    
    [Inject]
    private MapManager mapManager;

private void Awake()
    {
        //룸생성 및 던전패스파인더 룸리스트에 주입
        Init();
        BuildMap();
        FindPlayerSpawnPos();
        mapManager.TileMapDictInit(wideTileMap());
        mapManager.MapDictInit();
    }

    private List<Tilemap> wideTileMap()
    {
        List<Tilemap> wideTileMaps = new List<Tilemap>();
        for (int i = 0; i < RoomList.Count; i++) 
        { 
            List<Tilemap> tilemaps = RoomList[i].GetComponentsInChildren<Tilemap>().ToList();
            Tilemap largestTilemap = null; float maxArea = 0f;
            foreach (Tilemap tilemap in tilemaps)
            {
                Bounds b = tilemap.localBounds; float area = b.size.x * b.size.y;
                if (area > maxArea)
                {
                    maxArea = area; largestTilemap = tilemap;
                } 
                
            } 
            
            wideTileMaps.Add(largestTilemap); } return wideTileMaps;
    }

    /// <summary>
    /// 방 생성
    /// </summary>
	private void BuildMap()
    {
        if(IngameManager.instance != null)
            stageNum = IngameManager.instance.GetStage();
        
		for (int i = 0; i < roomPos.Count; i++)
        {
            //보스 방
            if (i == roomPos.Count - 1)
            {
                GameObject bossRoom = bossRoomsDict[stageNum.ToString()][0];
                RoomList.Add(container.InstantiatePrefab(bossRoom, roomPos[i].transform.position, Quaternion.identity, null));
                RoomList[i].GetComponent<Room>().SetBossRoom(true);
                RoomList[i].name = $"BossRoom";
            }
            //일반 방
            else
            {
                GameObject room = roomsDict[stageNum.ToString()][Random.Range(0, roomsDict[stageNum.ToString()].Count)];
                RoomList.Add(container.InstantiatePrefab(room, roomPos[i].transform.position, Quaternion.identity, null));
                RoomList[i].GetComponent<Room>().SetBossRoom(false);
                RoomList[i].name = $"Room_{i}";
            }

            //룸 리스트에 추가
            dungeonPathfinder.SetRoomList(RoomList[i].GetComponent<Grid>());
        }
	}
    
    private void FindPlayerSpawnPos()
    {
        if (RoomList.Count == 0)
        {
            Debug.LogWarning("RoomList 비어있음. 스폰 위치 찾기 실패");
            return;
        }

        GameObject firstRoom = RoomList[0];

        Tilemap floor = firstRoom.transform.GetChild(0).GetComponent<Tilemap>();
        Tilemap wall  = firstRoom.transform.GetChild(1).GetComponent<Tilemap>();

        if (floor == null)
        {
            Debug.LogWarning("첫 번째 방에 floor 타일맵 없음.");
            return;
        }

        List<Vector3Int> validCells = new List<Vector3Int>();
        BoundsInt bounds = floor.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (!floor.HasTile(pos)) continue;           // floor 있어야 하고
            if (wall != null && wall.HasTile(pos)) continue; // wall 없어야 함

            validCells.Add(pos);
        }

        if (validCells.Count == 0)
        {
            Debug.LogWarning("0번 방에서 스폰 가능한 타일 없음.");
            return;
        }

        // 랜덤 셀 선택
        Vector3Int cell = validCells[Random.Range(0, validCells.Count)];

        Vector3 worldPos = floor.CellToWorld(cell) + floor.cellSize / 2f;
        worldPos.z = 0;

        ApplyPlayerSpawnPos(worldPos);
    }
    
    private void ApplyPlayerSpawnPos(Vector3 pos)
    {
        PlayerRoot.PlayerRootInstance.TranslateScenePosition(pos);
    }

    /// <summary>
    /// 문 생성
    /// </summary>
    public void BuildDoor(List<Grid> route, bool isReverse)
    {
        foreach (Grid r in route)
        {
            GameObject room = r.gameObject;

            // 메인 루트: 마지막 방 제외
            if (!isReverse && room == RoomList[^1])
                continue;
            
            // 메인루트에서 사이드 루트의 마지막 방도 제외해야함
            if (!isReverse && room == route[^1].gameObject)
                continue;

            // 사이드 루트: route의 마지막 방 제외
            
            if (isReverse && room == route[^1].gameObject)
                continue;

            Tilemap floor = room.transform.GetChild(0).GetComponent<Tilemap>();
            Tilemap wall  = room.transform.GetChild(1).GetComponent<Tilemap>();

            if (floor == null) continue;

            List<Vector3> possibleDoorPositions = new List<Vector3>();
            BoundsInt bounds = floor.cellBounds;

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                // (1) 바닥 타일이어야 함
                if (!floor.HasTile(pos)) continue;

                // (2) 벽 타일이어서는 안 됨
                if (wall.HasTile(pos)) continue;
                

                // Cell → 월드 좌표 변환
                Vector3 worldPos = floor.CellToWorld(pos) + floor.cellSize / 2f;
                possibleDoorPositions.Add(new Vector3(worldPos.x, worldPos.y, 0));
            }

            if (possibleDoorPositions.Count > 0)
            {
                Vector3 doorPos = possibleDoorPositions[Random.Range(0, possibleDoorPositions.Count)];

                GameObject door = container.InstantiatePrefab(doorPrefab, doorPos, Quaternion.identity, room.transform);

                DungeonDoor d = door.GetComponent<DungeonDoor>();
                d.SetRoute(route);
                d.SetIsReverse(isReverse);

                if (isReverse)
                    container.InstantiatePrefab(doorMarker, doorPos, Quaternion.identity, room.transform);
            }
        }
    }
    
    

    private void SetPlayerSpawnPos(Vector3 pos)
    {
        PlayerRoot.PlayerRootInstance.TranslateScenePosition(pos);
    }
    

    private void Init()
    {
        roomsDict[nameof(StageNum.STAGE1)] = rooms_Stage1;
        roomsDict[nameof(StageNum.STAGE2)] = rooms_Stage2;
        roomsDict[nameof(StageNum.STAGE3)] = rooms_Stage3;
            
        bossRoomsDict[nameof(StageNum.STAGE1)] = bossRooms_Stage1;
        bossRoomsDict[nameof(StageNum.STAGE2)] = bossRooms_Stage2;
        bossRoomsDict[nameof(StageNum.STAGE3)] = bossRooms_Stage3;
    }
}
