using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class DungeonRoomSpawner : MonoBehaviour
{
    [Inject] protected DiContainer container;

    [SerializeField] private DungeonPathfinder dungeonPathfinder;

    //Todo : 스테이지에 따라 rooms 목록 변경
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

    [SerializeField] private StageNum stageNum;

    public List<GameObject> RoomList = new List<GameObject>();

    [SerializeField] private List<Grid> viewRoute = new List<Grid>();

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
        viewRoute = route;
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

                // (3) 바로 옆이 하나라도 벽이어야 함 (문은 방 외곽에 위치해야 하므로)
                bool isEdge =
                    wall.HasTile(pos + Vector3Int.up) ||
                    wall.HasTile(pos + Vector3Int.down) ||
                    wall.HasTile(pos + Vector3Int.left) ||
                    wall.HasTile(pos + Vector3Int.right);

                if (!isEdge) continue; // 외곽이 아니면 문 만들기 부적합

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
