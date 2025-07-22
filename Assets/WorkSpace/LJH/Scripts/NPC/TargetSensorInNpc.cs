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
    [SerializeField] private Vector3 castleDoorPos;
    [SerializeField] private Vector3 deskPos;
    [SerializeField] private Vector3 randomPos;
    [SerializeField] private Vector3 randomPosInStore;

    private Npc npc;

    public void Init(Npc npc)
    {
        gridList = new List<Grid>(storage.GetGridList());
        mapList = new List<Tilemap>(storage.GetMapTileList());
        obstacleList = new List<Tilemap>(storage.GetObstacleTileList());
        inStoreDoorPos = storage.OutsideDoorPos;
        outStoreDoorPos = storage.StoreDoorPos;
        castleDoorPos = storage.CastleDoorPos;
        deskPos = storage.DeskPos;
        StartCoroutine(RandominitCoroutine());


        this.npc = npc;
        UpdateGridBasedOnPosition();
    }
    private IEnumerator RandominitCoroutine()
    {
        while (true)
        {
            randomPos = storage.RandomPos;
            randomPosInStore = storage.RandomPosInStore;
            yield return new WaitForSeconds(1f);
        }
    }

    public Vector3 GetRandomPosition() => randomPos;
    public Vector3 GetRandomPositionInStore() => randomPosInStore;
    public Vector3 GetEnterPosition() => inStoreDoorPos;
    public Vector3 GetLeavePosition() => outStoreDoorPos;
    public Vector3 GetCastleDoorPosition() => castleDoorPos;
    public Vector3 GetDeskPosition() => deskPos;

    public Grid GetCurrentGrid(Vector3 worldPos)
    {
        foreach (var grid in gridList)
        {
            Tilemap tilemap = grid.transform.GetChild(0).GetComponent<UnityEngine.Tilemaps.Tilemap>();
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