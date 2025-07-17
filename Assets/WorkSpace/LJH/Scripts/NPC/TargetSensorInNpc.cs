using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class TargetSensorInNpc : MonoBehaviour
{
    [Inject] private TargetPosStorage storage;

    [SerializeField] private List<Grid> gridList = new();
    [SerializeField] private List<Tilemap> mapList = new();
    [SerializeField] private List<Tilemap> obstacleList = new();
    [SerializeField] private Vector3 inStoreDoorPos;
    [SerializeField] private Vector3 outStoreDoorPos;
    [SerializeField] private Vector3 randomPos;

    private Npc npc;

    public void Init(Npc npc)
    {
        gridList = new List<Grid>(storage.GetGridList());
        mapList = new List<Tilemap>(storage.GetMapTileList());
        obstacleList = new List<Tilemap>(storage.GetObstacleTileList());
        inStoreDoorPos = storage.OutsideDoorPos;
        outStoreDoorPos = storage.StoreDoorPos;
        StartCoroutine(RandominitCoroutine());


        this.npc = npc;
        UpdateGridBasedOnPosition();
    }
    private IEnumerator RandominitCoroutine()
    {
        while (true)
        {
            randomPos = storage.RandomPos;
            yield return new WaitForSeconds(1f);
        }
    }

    public Vector3 GetRandomPosition() => randomPos;

    public Vector3 GetEnterPosition() => inStoreDoorPos;
    public Vector3 GetLeavePosition() => outStoreDoorPos;

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

    public void UpdateGridBasedOnPosition()
    {
        Grid currentGrid = GetCurrentGrid(npc.transform.position);
        if (currentGrid != null)
        {
            npc.GetComponentInChildren<AstarPath>().SetGridAndTilemap(currentGrid);
        }
    }
}