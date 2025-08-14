using System.Collections.Generic;
using UnityEngine;

public class DungeonDoor : MonoBehaviour
{
    private DungeonPathfinder pathfinder;
    
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

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                other.gameObject.transform.position = toGrid.gameObject.transform.position;
            }
        }
    }
}
