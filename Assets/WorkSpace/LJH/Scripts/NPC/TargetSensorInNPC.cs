using AYellowpaper.SerializedCollections.Editor.Search;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject.SpaceFighter;
using static UnityEditor.PlayerSettings;

public class TargetSensorInNPC : MonoBehaviour
{
    [Header("Grid & TileMap / 0 = outside, 1 = store")]
    [SerializeField] private List<Grid> gridList = new List<Grid>();
    [SerializeField] private List<Tilemap> mapTile = new List<Tilemap>();
    [SerializeField] private List<Tilemap> obstacleTile = new List<Tilemap>();
    int outside = 0;
    int store = 1;

    [Header("Component")]
    [SerializeField] private NPC npc;
    [SerializeField] private AstarPath astar;
    [SerializeField] private NpcStateMachine state;

    [Header("Storage")]
    [SerializeField] private TargetPosStorage targetPosStorage;

    private GameObject table;

    private Vector3 castleDoorPos;
    private Vector3 outsideDoorPos;
    private Vector3 storeDoorPos;
    private Vector3 tablePos;
    private Vector3 playerPos;
    private Vector3 randomPos;

    [HideInInspector] public Vector3 targetPos;

    private void Start()
    {
        PosInit();
        astar.SetGridAndTilemap(gridList[outside]);
        StartCoroutine(SetTargetCoroutine());
        StartCoroutine(ChangeGridCoroutine());

    }

    private IEnumerator SetTargetCoroutine()
    {
        while(true)
        {
            if(!npc.GetMoving())
            {
                Set_Target();
            }
            yield return new WaitForSeconds(1f);
        }
    }
    public void InjectTable(Table table)
    {
        this.table = table.gameObject;
        tablePos = table.transform.position;
    }

    private void Set_Target()
    {
        npc.SetMoving(true);
        Vector3 npcPos = npc.transform.position;

        Grid curGrid = GetCurrentGridNpc(npcPos, gridList);

        Debug.Log($"{npc.name} 의 IsBuyer는 {npc.IsBuyer}");

        if (npc.IsBuyer)
        {
            Debug.Log($"curGrid = {curGrid}");
            Debug.Log($"gridList[store] = {gridList[store]}");
            if (curGrid == gridList[store])
            {
                Debug.Log("상점 그리드");
                //상태 패턴 또는 분기 전환식으로 타겟 바꿔주면 됨
                if (npc.wantItem != null)
                {
                    if (table != null)
                    {
                        Debug.Log("테이블 실행됨");
                        state.ChangeState(new NpcMoveState(npc, tablePos));
                    }
                    else if (table == null)
                    {
                        Debug.Log("플레이어 실행됨");
                        state.ChangeState(new NpcMoveState(npc, playerPos));
                    }
                }
                else
                {
                    Debug.Log("상점 퇴장 실행됨");
                    state.ChangeState(new NpcMoveState(npc, storeDoorPos));
                }
            }
            else
            {
                Debug.Log("외부 그리드");
                if (npc.wantItem != null)
                {
                    Debug.Log("상점 입장 실행됨");
                    state.ChangeState(new NpcMoveState(npc, outsideDoorPos));
                }

                else
                {
                    Debug.Log("성문 실행됨");
                    state.ChangeState(new NpcMoveState(npc, castleDoorPos));
                }
            }
        }
        else
        {
            Debug.Log("방황함");
            state.ChangeState(new NpcMoveState(npc, randomPos));
        }

    }
    public Grid GetCurrentGridNpc(Vector3 npcWorldPos, List<Grid> gridList)
    {
        foreach (Grid grid in gridList)
        {
            Tilemap tilemap = grid.transform.GetChild(0).GetComponent<Tilemap>(); // 타일맵
            Vector3Int cellPos = grid.WorldToCell(npcWorldPos);

            if (tilemap.cellBounds.Contains(cellPos))
            {
                return grid; // 이 그리드가 현재 위치
            }
        }

        return null; // 어디에도 없음
    }

    private IEnumerator ChangeGridCoroutine()
    {
        int num = outside;

        while (true)
        {
            yield return new WaitForSeconds(1f);

            astar.SetGridAndTilemap(gridList[num]);

            num = (num == store) ? store : outside;

        }

    }

    private void PosInit()
    {
        targetPosStorage = GameObject.Find("DayManager").GetComponent<TargetPosStorage>();

        gridList = targetPosStorage.SetGrid(gridList);
        mapTile = targetPosStorage.SetMaptile(mapTile);
        obstacleTile = targetPosStorage.SetObstacletile(obstacleTile);

        castleDoorPos = targetPosStorage.CastleDoor;
        outsideDoorPos = targetPosStorage.OutsideDoorPos;
        storeDoorPos = targetPosStorage.StoreDoorPos;
        playerPos = targetPosStorage.PlayerPos;

        StartCoroutine(RandominitCoroutine());
    }

    private IEnumerator RandominitCoroutine()
    {
        while(true)
        {
            randomPos = targetPosStorage.RandomPos;
            yield return new WaitForSeconds(1f);
        }
    }

    public void TableButton()
    {
        state.ChangeState(new NpcMoveState(npc, tablePos));
    }
    public void PlayerButton()
    {
        state.ChangeState(new NpcMoveState(npc, playerPos));
    }
    public void storeDoorButton()
    {
        state.ChangeState(new NpcMoveState(npc, storeDoorPos));
    }
    public void OutDoorButton()
    {
        state.ChangeState(new NpcMoveState(npc, outsideDoorPos));
    }
    public void CastleButton()
    {
        state.ChangeState(new NpcMoveState(npc, castleDoorPos));
    }
}
