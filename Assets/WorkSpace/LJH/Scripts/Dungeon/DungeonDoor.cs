using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class DungeonDoor : MonoBehaviour
{
    private DungeonPathfinder pathfinder;
    private Collider2D player;
    [SerializeField] private bool triggered = false;
    
    [Inject]
    MinimapController minimap;
    [Inject]
    MapManager mapManager;

    /// <summary>
    /// 목적지
    /// </summary>

    [SerializeField] private Room room;
    private Grid toGrid;
    
    [SerializeField] private List<Tilemap> floorTilemap =  new List<Tilemap>();

    [SerializeField] private bool isReverse;
    public void SetIsReverse(bool isReverse) => this.isReverse = isReverse;
    
    
    
    //테스트용

    [SerializeField] private Grid testToGrid;
    private void Start()
    {
        StartCoroutine(RoomFindCoroutine());
    }

    /// <summary>
    /// 최악의 방식이지만.. 나중에 시간이 나면 개선할 것
    /// </summary>
    /// <returns></returns>
    private IEnumerator RoomFindCoroutine()
    {
        yield return new WaitForSeconds(0.05f);
        floorTilemap.Add(GameObject.Find("Room_0").transform.GetChild(0).GetComponent<Tilemap>());
        floorTilemap.Add(GameObject.Find("Room_1").transform.GetChild(0).GetComponent<Tilemap>());
        floorTilemap.Add(GameObject.Find("Room_2").transform.GetChild(0).GetComponent<Tilemap>());
        floorTilemap.Add(GameObject.Find("Room_3").transform.GetChild(0).GetComponent<Tilemap>());
        floorTilemap.Add(GameObject.Find("Room_4").transform.GetChild(0).GetComponent<Tilemap>());
        floorTilemap.Add(GameObject.Find("BossRoom").transform.GetChild(0).GetComponent<Tilemap>());
    }

    
    
/// <summary>
/// 해당 문이 가져올 루트 설정
/// </summary>
/// <param name="route">메인or사이드</param>
    public void SetRoute(List<Grid> route)
    {
        SetToGrid(route);
    }

    private void SetToGrid(List<Grid> route)
    {
        Grid curGrid = transform.GetComponentInParent<Grid>();
        
        int curGridIndex = route.IndexOf(curGrid);

        //마지막 방의 경우 목적지 필요없음
        if (curGridIndex == route.Count - 1) return;
        
        if (!isReverse)
        {
            // 순방향: 다음 방 (인덱스 + 1)
            if (curGridIndex < route.Count - 1)
            {
                toGrid = route[curGridIndex + 1];
            }
            else
            {
                toGrid = null;
            }
        }
        else
        {
            // 역방향: 이전 방 (인덱스 - 1)
            if (curGridIndex > 0)
            {
                toGrid = route[curGridIndex - 1];
            }
            else
            {
                toGrid = null; 
            }
        }
        
        room = FindRoomByGridPosition(toGrid.transform.position);
        
    }
    
    private Room FindRoomByGridPosition(Vector3 gridPos)
    {
        Room[] rooms = FindObjectsOfType<Room>();

        foreach (Room r in rooms)
        {
            // 위치가 정확히 같으면 같은 방이다
            if (Vector3.Distance(r.transform.position, gridPos) < 0.1f)
            {
                return r;
            }
        }

        return null; // 못 찾았을 경우
    }
    

    private void Update()
    {
        if (player != null && triggered)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            MapGridChecker();
            minimap.CamPosSet(room.transform.position);
            
            Vector3 randomPos = GetRandomFloorPosition(room);
            
            Rigidbody2D rb = player.attachedRigidbody;
            rb.position = randomPos;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
    
    private Vector3 GetRandomFloorPosition(Room targetRoom)
    {
        Tilemap floorTilemap = targetRoom.transform.GetChild(0).GetComponent<Tilemap>(); // 방의 바닥 타일맵
        Tilemap wallTilemap  = targetRoom.transform.GetChild(1).GetComponent<Tilemap>(); // 벽 타일맵

        List<Vector3> floorPositions = new List<Vector3>();

        BoundsInt bounds = floorTilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            // 바닥 타일이어야 하고
            if (!floorTilemap.HasTile(pos)) continue;
        
            // 벽이 아니어야 함
            if (wallTilemap.HasTile(pos)) continue;

            // Cell → World 변환
            Vector3 worldPos = floorTilemap.CellToWorld(pos) + floorTilemap.cellSize / 2f;
            floorPositions.Add(worldPos);
        }

        // 예외 처리
        if (floorPositions.Count == 0)
            return targetRoom.transform.position;

        // 랜덤 선택
        return floorPositions[Random.Range(0, floorPositions.Count)];
    }
    
    
    
    
    
    
    
    
    /// <summary>
    /// 목적지를 대조하여 맞는 방위치로 카메라 맵을 변경해주는 메서드
    /// </summary>
    private void MapGridChecker()
    {
        
        Vector3 checkPosition = toGrid.transform.position;


        foreach (Tilemap tilemap in floorTilemap)
        {
            Vector3Int gridPosition = tilemap.WorldToCell(checkPosition);
            if (tilemap.HasTile(gridPosition))
            {
                mapManager.MapChange((MapType)floorTilemap.IndexOf(tilemap) + 3);
            }
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision;
            triggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player = null;
            triggered = false;
        }
    }
    
}
