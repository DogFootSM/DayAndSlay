using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonRoomSpawner : MonoBehaviour
{
    [SerializeField] private DungeonPathfinder dungeonPathfinder;
    //Todo : 스테이지에 따라 rooms 목록 변경
    [SerializeField] private List<GameObject> rooms;
    [SerializeField] private List<GameObject> bossRooms;
    [SerializeField] private List<GameObject> roomPos;
    
    [SerializeField] private GameObject doorPrefab;

    public List<GameObject> RoomList = new List<GameObject>();

    private void Awake()
    {
        //룸생성 및 던전패스파인더 룸리스트에 주입
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
                GameObject bossRoom = bossRooms[0];
                RoomList.Add(Instantiate(bossRoom, roomPos[i].transform.position, Quaternion.identity));
                RoomList[RoomList.Count - 1].GetComponent<Room>().SetBossRoom(true);
            }
            //일반 방
            else
            {
                GameObject room = rooms[Random.Range(0, rooms.Count)];
                RoomList.Add(Instantiate(room, roomPos[i].transform.position, Quaternion.identity));
                RoomList[i].GetComponent<Room>().SetBossRoom(false);
            }
            //룸 리스트에 추가
            dungeonPathfinder.SetRoomList(RoomList[i].GetComponent<Grid>());
        }
	}

    /// <summary>
    /// 문 생성
    /// </summary>
    public void BuildDoor(List<Grid> route)
    {
        foreach (Grid r in route)
        {
            GameObject room = r.gameObject;
            
            if (room == RoomList[RoomList.Count - 1]) continue;
            
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
                    floorPositions.Add(worldPos);
                }
            }

            if (floorPositions.Count > 0)
            {
                Vector3 doorPos = floorPositions[Random.Range(0, floorPositions.Count)];

                Instantiate(doorPrefab, doorPos, Quaternion.identity, room.transform);
            }
        }
    }
}
