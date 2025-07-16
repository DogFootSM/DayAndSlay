using System.Collections.Generic;
using UnityEngine;

public class TargetSensorNew : MonoBehaviour
{
    [SerializeField] private List<Grid> gridList;
    [SerializeField] private List<Vector3> doorPositions;

    private NpcNew npc;

    public void Init(NpcNew npc)
    {
        this.npc = npc;
    }

    public Vector3 GetLeavePosition()
    {
        // 단순히 외부로 나가는 문 위치 반환 (예시: 첫 번째 문)
        return doorPositions.Count > 0 ? doorPositions[0] : npc.transform.position;
    }

    public Grid GetCurrentGrid(Vector3 worldPos)
    {
        foreach (var grid in gridList)
        {
            var tilemap = grid.transform.GetChild(0).GetComponent<UnityEngine.Tilemaps.Tilemap>();
            Vector3Int cell = grid.WorldToCell(worldPos);
            if (tilemap.cellBounds.Contains(cell)) return grid;
        }
        return null;
    }
}