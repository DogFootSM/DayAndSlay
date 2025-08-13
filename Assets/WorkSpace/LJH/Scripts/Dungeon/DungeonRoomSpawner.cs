using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomSpawner : MonoBehaviour
{
    [SerializeField] private DungeonPathfinder dungeonPathfinder;
    //Todo : 스테이지에 따라 rooms 목록 변경
    [SerializeField] private List<GameObject> rooms;
    [SerializeField] private List<GameObject> bossRooms;
    [SerializeField] private List<GameObject> roomPos;

    public List<GameObject> RoomList = new List<GameObject>();

    private void Awake()
    {
        //룸생성 및 던전패스파인더 룸리스트에 주입
        for (int i = 0; i < roomPos.Count; i++)
        {
            if (i == roomPos.Count - 1)
            {
                GameObject bossRoom = bossRooms[0];
                RoomList.Add(Instantiate(bossRoom, roomPos[i].transform.position, Quaternion.identity));
                RoomList[RoomList.Count - 1].GetComponent<Room>().SetBossRoom(true);
            }

            else
            {
                GameObject room = rooms[Random.Range(0, rooms.Count)];
                RoomList.Add(Instantiate(room, roomPos[i].transform.position, Quaternion.identity));
                RoomList[i].GetComponent<Room>().SetBossRoom(false);
            }
            
            dungeonPathfinder.SetRoomList(RoomList[i].GetComponent<Grid>());
        }
    }
}
