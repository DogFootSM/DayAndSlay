using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DungeonDoor : MonoBehaviour
{
/*
    [SerializeField] Transform spawner;
    [HideInInspector] public Vector2 spawnerPos;

    List<GameObject> doorList;

    Grid fromGrid;
    Grid toGrid;

    //인덱스 이동용
    private int verticalOffset = 3;
    private int horizonOffset = 1;
    private int index;

    private bool isFirst;

    public bool ThisDoorIsFirst(bool yes) => isFirst;

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
        if (isFirst)
        {
            toGrid = [index + 1];
        }

        else
        {
            toGrid = roomList[index - 1];
        }
        
        
        
        
        
        

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
        fromGrid = GetComponentInParent<Grid>();
        index = roomList.IndexOf(fromGrid);
        doorList = GetComponentInParent<Room>().GetDoorList();
    }*/
}
