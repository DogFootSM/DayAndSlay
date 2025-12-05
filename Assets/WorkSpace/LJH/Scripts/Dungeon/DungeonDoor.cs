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
            if (room.name == "BossRoom")
            {
                SoundManager.Instance.PlayBGM(BGMSound.BOSSROOM);
            }
            
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
    
    
    
    
    
    
    
    /*
    /// <summary>
    /// 목적지를 대조하여 맞는 방위치로 카메라 맵을 변경해주는 메서드
    /// </summary>
    private void MapGridChecker()
    {
        
        Vector3 checkPosition = toGrid.transform.position;
        Debug.Log($"맵 포커싱 변경 시도1");
        Debug.Log($"체크 포지션 = {checkPosition}");

        foreach (Tilemap tilemap in floorTilemap)
        {
            Debug.Log($"{(MapType)floorTilemap.IndexOf(tilemap) + 3}에서 맵 포커싱 변경 시도");
            Vector3Int gridPosition = tilemap.WorldToCell(checkPosition);
            Debug.Log($"무슨 타일맵인가 = {tilemap.transform.parent.name}");
            Debug.Log($"뚫었는가? {tilemap.HasTile(gridPosition)}");
            if (tilemap.HasTile(gridPosition))
            {
                Debug.Log($"{(MapType)floorTilemap.IndexOf(tilemap) + 3}로 맵 포커싱 변경");
                mapManager.MapChange((MapType)floorTilemap.IndexOf(tilemap) + 3);
            }
        }
    }*/
    
    private void MapGridChecker()
    {
        // 타일 체크 기준이 되는 월드 좌표
        Vector3 checkPosition = toGrid.transform.position;
        Debug.Log($"[체커] 맵 포커싱 검사 시작 / 월드 좌표 = {checkPosition}");

        foreach (Tilemap tilemap in floorTilemap)
        {
            int idx = floorTilemap.IndexOf(tilemap);
            MapType type = (MapType)(idx + 3);

            Debug.Log($"============================");
            Debug.Log($"[체커] {type} 타일맵 검사 시작");
            Debug.Log($"현재 검사 중인 타일맵 오브젝트 = {tilemap.transform.parent.name}");

            // 월드 좌표를 타일 좌표로 변환
            Vector3Int gridPosition = tilemap.WorldToCell(checkPosition);
            Debug.Log($"타일 좌표(gridPosition) = {gridPosition}");

            // 타일맵의 전체 영역 (타일이 존재할 수 있는 범위)
            Debug.Log($"타일맵 cellBounds = {tilemap.cellBounds}");

            // 변환된 gridPosition이 cellBounds 안에 포함되는지 검사
            Debug.Log($"cellBounds 안에 포함됨? {tilemap.cellBounds.Contains(gridPosition)}");

            // 실제로 그 위치에 타일이 있는지 검사
            bool hasTile = tilemap.HasTile(gridPosition);
            Debug.Log($"HasTile 결과 = {hasTile}");

            // 그 좌표의 실제 타일 객체를 가져와 확인
            var tile = tilemap.GetTile(gridPosition);
            Debug.Log($"GetTile 결과 = {(tile == null ? "NULL (타일 없음)" : tile.name)}");

            // 타일이 존재하면 맵 전환 실행
            if (hasTile)
            {
                Debug.Log($"★★ 타일 발견! {type} 맵으로 포커싱 변경 ★★");
                mapManager.MapChange(type);
            }

            Debug.Log($"============================\n");
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
