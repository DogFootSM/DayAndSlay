using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class DungeonDoor : MonoBehaviour
{
    private DungeonPathfinder pathfinder;
    private Collider2D player;
    private bool triggered = false;
    
    [Inject]
    MinimapController minimap;
    [Inject]
    MapManager mapManager;
    
    /// <summary>
    /// 목적지
    /// </summary>
    private Grid toGrid;
    
    
    //테스트용

    private void Start()
    {
        
    }

    private IEnumerator CoCoCo()
    {
        yield return new WaitForSeconds(0.05f);
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
        
        toGrid = route[curGridIndex + 1];
        
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MapGridChecker();
            minimap.CamPosSet(toGrid.transform.position);
            Rigidbody2D rb = player.attachedRigidbody;
            rb.position = toGrid.gameObject.transform.position;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    private List<Tilemap> floorTilemap =  new List<Tilemap>();
    
    /// <summary>
    /// 목적지를 대조하여 맞는 방위치로 카메라 맵을 변경해주는 메서드
    /// </summary>
    private void MapGridChecker()
    {
        Vector3 checkPosition = toGrid.transform.position;

        Vector3Int gridPosition = floorTilemap[0].WorldToCell(checkPosition);

        foreach (Tilemap tilemap in floorTilemap)
        {
            if (tilemap.HasTile(gridPosition))
            {
                mapManager.MapChange((MapType)floorTilemap.IndexOf(tilemap) + 2);
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
