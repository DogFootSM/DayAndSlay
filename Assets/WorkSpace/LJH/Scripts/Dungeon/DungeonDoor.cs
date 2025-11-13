using System.Collections.Generic;
using UnityEngine;
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
            minimap.CamPosSet(toGrid.transform.position);
            Rigidbody2D rb = player.attachedRigidbody;
            rb.position = toGrid.gameObject.transform.position;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            mapManager.MapChange(MapType.DUNGEON_0);
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
