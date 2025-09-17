using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Zenject;

public class DungeonRoomSpawner : MonoBehaviour
{
    [Inject] protected DiContainer container;
    
    [SerializeField] private DungeonPathfinder dungeonPathfinder;
    //Todo : 스테이지에 따라 rooms 목록 변경
    [SerializeField][SerializedDictionary] private SerializedDictionary<string, List<GameObject>> roomsDict;
    [SerializeField][SerializedDictionary] private SerializedDictionary<string, List<GameObject>> bossRoomsDict;
    
    [SerializeField] private List<GameObject> rooms_Stage1;
    [SerializeField] private List<GameObject> rooms_Stage2;
    [SerializeField] private List<GameObject> rooms_Stage3;
    
    [SerializeField] private List<GameObject> bossRooms_Stage1;
    [SerializeField] private List<GameObject> bossRooms_Stage2;
    [SerializeField] private List<GameObject> bossRooms_Stage3;
    
    [SerializeField] private List<GameObject> roomPos;
    
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private GameObject doorMarker;
    
    [SerializeField] private StageNum stageNum;

    public List<GameObject> RoomList = new List<GameObject>();

    private void Awake()
    {
        //룸생성 및 던전패스파인더 룸리스트에 주입
        Init();
        BuildMap();
    }

    /// <summary>
    /// 방 생성
    /// </summary>
	private void BuildMap()
	{
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

    /// <summary>
    /// 문 생성
    /// </summary>
    public void BuildDoor(List<Grid> route, bool isReverse)
    {
        foreach (Grid r in route)
        {
            GameObject room = r.gameObject;
            
            //맨끝방의 도어와 시작방의 이전 문은 생성하지 않음
            if (room == RoomList[^1]) continue;
            if (room == route[^1].gameObject && isReverse) continue;
            
            Tilemap floorTilemap = room.transform.GetChild(1).GetComponent<Tilemap>();
            Tilemap wallTilemap = room.transform.GetChild(0).GetComponent<Tilemap>();
            
            if (floorTilemap == null) continue;

            List<Vector3> floorPositions = new List<Vector3>();
            
            BoundsInt bounds = floorTilemap.cellBounds;
            
            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                if (floorTilemap.HasTile(pos) && !wallTilemap.HasTile(pos))
                {
                    // Cell -> World 좌표 변환
                    Vector3 worldPos = floorTilemap.CellToWorld(pos) + floorTilemap.cellSize / 2f;
                    floorPositions.Add(new Vector3(worldPos.x, worldPos.y, 0));
                }
            }

            if (floorPositions.Count > 0)
            {
                Vector3 doorPos = floorPositions[Random.Range(0, floorPositions.Count)];

                GameObject door = container.InstantiatePrefab(doorPrefab, doorPos, Quaternion.identity, room.transform);
                
                DungeonDoor ddoor = door.GetComponent<DungeonDoor>();
                
                ddoor.SetRoute(route);

                if (isReverse)
                {
                    container.InstantiatePrefab(doorMarker, doorPos, Quaternion.identity, room.transform);
                }
            }
        }
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
