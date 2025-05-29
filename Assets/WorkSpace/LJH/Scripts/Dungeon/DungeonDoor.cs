using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DungeonDoor : MonoBehaviour
{
    [Inject]
    private DungeonPathfinder dungeonPathfinder;

    [SerializeField] Transform spawner;
    [HideInInspector] public Vector2 spawnerPos;

    List<Grid> roomList;

    List<GameObject> doorList;

    Grid fromGrid;
    Grid toGrid;

    //인덱스 이동용
    int verticalOffset = 4;
    int horizonOffset = 1;
    int index;

    private void Start()
    {
        Init();
        DoorPathfinder();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.transform.position = spawnerPos;
        }
    }

    /// <summary>
    /// 문의 목적지를 정해주는 함수
    /// </summary>
    void DoorPathfinder()
    {

        for (int i = 0; i < doorList.Count; i++)
        {
            if (!doorList[i].activeSelf)
            {
                continue;
            }

            Direction dir = (Direction)i;

            if (gameObject == doorList[i])
            {
                switch (dir)
                {
                    case Direction.Up:
                        toGrid = roomList[index - verticalOffset];
                        break;

                    case Direction.Right:
                        toGrid = roomList[index + horizonOffset];
                        break;

                    case Direction.Down:
                        toGrid = roomList[index + verticalOffset];
                        break;

                    case Direction.Left:
                        toGrid = roomList[index - horizonOffset];
                        break;
                }
                break;
            }
        }
        // spawnerPos는 목적지 그리드에 있는 spawnerPos
        spawnerPos = toGrid.GetComponentInChildren<DungeonDoor>().spawner.position;

    }

    void Init()
    {
        roomList = dungeonPathfinder.GetRoomList();
        fromGrid = GetComponentInParent<Grid>();
        index = roomList.IndexOf(fromGrid);
        doorList = GetComponentInParent<Room>().GetDoorList();
    }
}
